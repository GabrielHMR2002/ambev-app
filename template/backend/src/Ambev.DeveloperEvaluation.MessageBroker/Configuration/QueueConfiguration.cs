namespace Ambev.DeveloperEvaluation.MessageBroker.Configuration;

/// <summary>
/// Configuration for a RabbitMQ queue
/// </summary>
public class QueueConfiguration
{
    public string QueueName { get; set; } = string.Empty;
    public List<string> RoutingKeys { get; set; } = new();
    public bool Durable { get; set; } = true;
    public bool Exclusive { get; set; } = false;
    public bool AutoDelete { get; set; } = false;
    public Dictionary<string, object> Arguments { get; set; } = new();
}

/// <summary>
/// Defines all queue configurations for the application
/// </summary>
public static class QueueConfigurations
{
    /// <summary>
    /// Gets all configured queues for the sales events
    /// </summary>
    public static List<QueueConfiguration> GetAllQueues()
    {
        return new List<QueueConfiguration>
        {
            // Queue for all sale events (analytics, reporting)
            new QueueConfiguration
            {
                QueueName = "sales.all-events",
                RoutingKeys = new List<string> { "sale.#" },
                Durable = true,
                Arguments = new Dictionary<string, object>
                {
                    { "x-message-ttl", 86400000 }, // 24 hours
                    { "x-max-length", 10000 }
                }
            },

            // Queue specifically for sale creation events
            new QueueConfiguration
            {
                QueueName = "sales.created",
                RoutingKeys = new List<string> { "sale.created" },
                Durable = true,
                Arguments = new Dictionary<string, object>
                {
                    { "x-message-ttl", 86400000 }
                }
            },

            // Queue specifically for sale modification events
            new QueueConfiguration
            {
                QueueName = "sales.modified",
                RoutingKeys = new List<string> { "sale.modified" },
                Durable = true,
                Arguments = new Dictionary<string, object>
                {
                    { "x-message-ttl", 86400000 }
                }
            },

            // Queue for cancellation events (sales and items)
            new QueueConfiguration
            {
                QueueName = "sales.cancellations",
                RoutingKeys = new List<string> { "sale.cancelled", "sale.item.cancelled" },
                Durable = true,
                Arguments = new Dictionary<string, object>
                {
                    { "x-message-ttl", 86400000 }
                }
            },

            // Queue for notifications (created and cancelled events)
            new QueueConfiguration
            {
                QueueName = "sales.notifications",
                RoutingKeys = new List<string> { "sale.created", "sale.cancelled" },
                Durable = true,
                Arguments = new Dictionary<string, object>
                {
                    { "x-message-ttl", 3600000 } // 1 hour
                }
            },

            // Queue for audit (all events)
            new QueueConfiguration
            {
                QueueName = "sales.audit",
                RoutingKeys = new List<string> { "#" },
                Durable = true,
                Arguments = new Dictionary<string, object>
                {
                    { "x-message-ttl", 2592000000 } // 30 days
                }
            }
        };
    }
}