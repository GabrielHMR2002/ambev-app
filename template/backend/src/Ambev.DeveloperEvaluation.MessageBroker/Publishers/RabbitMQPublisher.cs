using Ambev.DeveloperEvaluation.MessageBroker.Configuration;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.MessageBroker.Publishers;

/// <summary>
/// RabbitMQ implementation of the message publisher
/// </summary>
public class RabbitMQPublisher : IMessagePublisher, IDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly object _lock = new();
    private bool _infrastructureCreated = false;

    public RabbitMQPublisher(RabbitMQSettings settings, ILogger<RabbitMQPublisher> logger)
    {
        _settings = settings;
        _logger = logger;

        if (_settings.Enabled)
        {
            InitializeConnection();
            if (_settings.AutoCreateQueues)
            {
                CreateInfrastructure();
            }
        }
    }

    /// <summary>
    /// Initializes the RabbitMQ connection and channel
    /// </summary>
    private void InitializeConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };

            _connection = factory.CreateConnection("SalesEventsPublisher");
            _channel = _connection.CreateModel();

            _logger.LogInformation(
                "RabbitMQ connection established successfully. Host: {HostName}:{Port}",
                _settings.HostName,
                _settings.Port
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize RabbitMQ connection");
            throw;
        }
    }

    /// <summary>
    /// Creates all necessary RabbitMQ infrastructure (exchanges, queues, bindings)
    /// </summary>
    private void CreateInfrastructure()
    {
        if (_infrastructureCreated || _channel == null)
            return;

        try
        {
            _logger.LogInformation("Creating RabbitMQ infrastructure...");

            // Create main exchange
            _channel.ExchangeDeclare(
                exchange: _settings.ExchangeName,
                type: _settings.ExchangeType,
                durable: true,
                autoDelete: false,
                arguments: null
            );
            _logger.LogInformation("Exchange created: {ExchangeName} (type: {ExchangeType})", 
                _settings.ExchangeName, _settings.ExchangeType);

            // Create Dead Letter Exchange
            _channel.ExchangeDeclare(
                exchange: _settings.DeadLetterExchangeName,
                type: "direct",
                durable: true,
                autoDelete: false,
                arguments: null
            );
            _logger.LogInformation("Dead Letter Exchange created: {DeadLetterExchange}", 
                _settings.DeadLetterExchangeName);

            // Create Dead Letter Queue
            var dlqArgs = new Dictionary<string, object>
            {
                { "x-queue-type", "classic" }
            };
            _channel.QueueDeclare(
                queue: _settings.DeadLetterQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: dlqArgs
            );
            _logger.LogInformation("Dead Letter Queue created: {DeadLetterQueue}", 
                _settings.DeadLetterQueueName);

            // Bind Dead Letter Queue to Dead Letter Exchange
            _channel.QueueBind(
                queue: _settings.DeadLetterQueueName,
                exchange: _settings.DeadLetterExchangeName,
                routingKey: "#"
            );

            // Create all configured queues and bindings
            var queueConfigurations = QueueConfigurations.GetAllQueues();
            foreach (var queueConfig in queueConfigurations)
            {
                CreateQueueWithBindings(queueConfig);
            }

            _infrastructureCreated = true;
            _logger.LogInformation(
                "RabbitMQ infrastructure created successfully. " +
                "Exchange: {Exchange}, Queues: {QueueCount}, DLQ: {DLQ}",
                _settings.ExchangeName,
                queueConfigurations.Count,
                _settings.DeadLetterQueueName
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create RabbitMQ infrastructure");
            throw;
        }
    }

    /// <summary>
    /// Creates a queue with all its bindings
    /// </summary>
    private void CreateQueueWithBindings(QueueConfiguration config)
    {
        if (_channel == null)
            return;

        try
        {
            // Add dead letter exchange to queue arguments
            var queueArgs = new Dictionary<string, object>(config.Arguments)
            {
                { "x-dead-letter-exchange", _settings.DeadLetterExchangeName },
                { "x-queue-type", "classic" }
            };

            // Declare the queue
            _channel.QueueDeclare(
                queue: config.QueueName,
                durable: config.Durable,
                exclusive: config.Exclusive,
                autoDelete: config.AutoDelete,
                arguments: queueArgs
            );

            _logger.LogInformation("Queue created: {QueueName}", config.QueueName);

            // Create bindings for each routing key
            foreach (var routingKey in config.RoutingKeys)
            {
                _channel.QueueBind(
                    queue: config.QueueName,
                    exchange: _settings.ExchangeName,
                    routingKey: routingKey
                );

                _logger.LogInformation(
                    "Binding created: Queue '{QueueName}' -> Exchange '{Exchange}' with routing key '{RoutingKey}'",
                    config.QueueName,
                    _settings.ExchangeName,
                    routingKey
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create queue {QueueName}", config.QueueName);
            throw;
        }
    }

    /// <summary>
    /// Ensures the connection and channel are available
    /// </summary>
    private void EnsureConnection()
    {
        if (_connection == null || !_connection.IsOpen || _channel == null || !_channel.IsOpen)
        {
            lock (_lock)
            {
                if (_connection == null || !_connection.IsOpen || _channel == null || !_channel.IsOpen)
                {
                    _logger.LogWarning("RabbitMQ connection lost. Attempting to reconnect...");
                    Dispose();
                    InitializeConnection();
                    _infrastructureCreated = false;
                    if (_settings.AutoCreateQueues)
                    {
                        CreateInfrastructure();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Publishes a message to RabbitMQ
    /// </summary>
    public async Task PublishAsync<T>(T message, string routingKey) where T : class
    {
        await PublishAsync(message, routingKey, null, null);
    }

    /// <summary>
    /// Publishes a message to RabbitMQ with additional properties
    /// </summary>
    public async Task PublishAsync<T>(T message, string routingKey, string? messageId = null, string? correlationId = null) where T : class
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("RabbitMQ is disabled. Message not published: {RoutingKey}", routingKey);
            return;
        }

        try
        {
            await Task.Run(() =>
            {
                EnsureConnection();

                var jsonMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                });

                var body = Encoding.UTF8.GetBytes(jsonMessage);

                var properties = _channel!.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";
                properties.ContentEncoding = "utf-8";
                properties.DeliveryMode = 2; 
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                properties.MessageId = messageId ?? Guid.NewGuid().ToString();
                properties.AppId = "Ambev.DeveloperEvaluation";
                properties.Type = typeof(T).Name;
                
                if (!string.IsNullOrEmpty(correlationId))
                {
                    properties.CorrelationId = correlationId;
                }

                properties.Headers = new Dictionary<string, object>
                {
                    { "published-at", DateTime.UtcNow.ToString("O") },
                    { "publisher", "SalesService" },
                    { "version", "1.0" }
                };

                _channel.BasicPublish(
                    exchange: _settings.ExchangeName,
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: body
                );

                _logger.LogInformation(
                    "Message published successfully. Exchange: {Exchange}, RoutingKey: {RoutingKey}, " +
                    "MessageId: {MessageId}, Size: {Size} bytes",
                    _settings.ExchangeName,
                    routingKey,
                    properties.MessageId,
                    body.Length
                );
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to publish message. Exchange: {Exchange}, RoutingKey: {RoutingKey}",
                _settings.ExchangeName,
                routingKey
            );
            throw;
        }
    }

    /// <summary>
    /// Disposes the RabbitMQ connection and channel
    /// </summary>
    public void Dispose()
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
            
            _logger.LogInformation("RabbitMQ connection disposed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ connection");
        }
    }
}