namespace Ambev.DeveloperEvaluation.MessageBroker.Interfaces;

/// <summary>
/// Interface for publishing messages to a message broker
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the specified routing key
    /// </summary>
    /// <typeparam name="T">The type of message to publish</typeparam>
    /// <param name="message">The message object to publish</param>
    /// <param name="routingKey">The routing key for message routing</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task PublishAsync<T>(T message, string routingKey) where T : class;

    /// <summary>
    /// Publishes a message to the specified routing key with additional properties
    /// </summary>
    /// <typeparam name="T">The type of message to publish</typeparam>
    /// <param name="message">The message object to publish</param>
    /// <param name="routingKey">The routing key for message routing</param>
    /// <param name="messageId">Optional message identifier</param>
    /// <param name="correlationId">Optional correlation identifier for tracking</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task PublishAsync<T>(T message, string routingKey, string? messageId = null, string? correlationId = null) where T : class;
}