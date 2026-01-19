using Ambev.DeveloperEvaluation.MessageBroker.Initialization;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.MessageBroker.Configuration;

/// <summary>
/// Extension methods for configuring message broker services
/// </summary>
public static class MessageBrokerConfiguration
{
    /// <summary>
    /// Adds RabbitMQ message broker services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = new RabbitMQSettings();
        configuration.GetSection(RabbitMQSettings.SectionName).Bind(settings);
        services.AddSingleton(settings);

        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

        services.AddHostedService<RabbitMQInitializer>();

        return services;
    }
}