namespace Ambev.DeveloperEvaluation.MessageBroker.Configuration;

/// <summary>
/// Configuration settings for RabbitMQ connection
/// </summary>
public class RabbitMQSettings
{
    public const string SectionName = "RabbitMQ";

    /// <summary>
    /// RabbitMQ server hostname
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// RabbitMQ server port
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Username for RabbitMQ authentication
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Password for RabbitMQ authentication
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Virtual host name
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Exchange name for publishing messages
    /// </summary>
    public string ExchangeName { get; set; } = "sales.events";

    /// <summary>
    /// Exchange type (direct, topic, fanout, headers)
    /// </summary>
    public string ExchangeType { get; set; } = "topic";

    /// <summary>
    /// Enable/disable message publishing
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Enable/disable automatic queue and binding creation
    /// </summary>
    public bool AutoCreateQueues { get; set; } = true;

    /// <summary>
    /// Dead letter exchange name
    /// </summary>
    public string DeadLetterExchangeName { get; set; } = "sales.events.dlx";

    /// <summary>
    /// Dead letter queue name
    /// </summary>
    public string DeadLetterQueueName { get; set; } = "sales.events.dlq";
}