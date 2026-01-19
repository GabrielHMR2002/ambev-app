using Ambev.DeveloperEvaluation.MessageBroker.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Ambev.DeveloperEvaluation.MessageBroker.Initialization;

/// <summary>
/// Background service that ensures RabbitMQ infrastructure is created on startup
/// </summary>
public class RabbitMQInitializer : IHostedService
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQInitializer> _logger;

    public RabbitMQInitializer(RabbitMQSettings settings, ILogger<RabbitMQInitializer> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_settings.Enabled)
        {
            _logger.LogInformation("RabbitMQ is disabled. Skipping infrastructure initialization.");
            return;
        }

        await Task.Run(() =>
        {
            try
            {
                _logger.LogInformation("Starting RabbitMQ infrastructure initialization...");

                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    Port = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    VirtualHost = _settings.VirtualHost
                };

                using var connection = factory.CreateConnection("InfrastructureInitializer");
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(
                    exchange: _settings.ExchangeName,
                    type: _settings.ExchangeType,
                    durable: true,
                    autoDelete: false
                );

                channel.ExchangeDeclare(
                    exchange: _settings.DeadLetterExchangeName,
                    type: "direct",
                    durable: true,
                    autoDelete: false
                );

                channel.QueueDeclare(
                    queue: _settings.DeadLetterQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false
                );

                channel.QueueBind(
                    queue: _settings.DeadLetterQueueName,
                    exchange: _settings.DeadLetterExchangeName,
                    routingKey: "#"
                );

                if (_settings.AutoCreateQueues)
                {
                    var queues = QueueConfigurations.GetAllQueues();
                    foreach (var queueConfig in queues)
                    {
                        var args = new Dictionary<string, object>(queueConfig.Arguments)
                        {
                            { "x-dead-letter-exchange", _settings.DeadLetterExchangeName }
                        };

                        channel.QueueDeclare(
                            queue: queueConfig.QueueName,
                            durable: queueConfig.Durable,
                            exclusive: queueConfig.Exclusive,
                            autoDelete: queueConfig.AutoDelete,
                            arguments: args
                        );

                        foreach (var routingKey in queueConfig.RoutingKeys)
                        {
                            channel.QueueBind(
                                queue: queueConfig.QueueName,
                                exchange: _settings.ExchangeName,
                                routingKey: routingKey
                            );
                        }

                        _logger.LogInformation(
                            "Queue '{QueueName}' created with {BindingCount} bindings",
                            queueConfig.QueueName,
                            queueConfig.RoutingKeys.Count
                        );
                    }
                }

                _logger.LogInformation(
                    "RabbitMQ infrastructure initialized successfully. " +
                    "Exchange: {Exchange}, DLX: {DLX}, DLQ: {DLQ}",
                    _settings.ExchangeName,
                    _settings.DeadLetterExchangeName,
                    _settings.DeadLetterQueueName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize RabbitMQ infrastructure");
                throw;
            }
        }, cancellationToken);
    }
    /// <summary>
    /// Stops the RabbitMQ initializer service
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ initializer stopped");
        return Task.CompletedTask;
    }
}