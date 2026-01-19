namespace Ambev.DeveloperEvaluation.MessageBroker.Messages;

/// <summary>
/// Message published when a sale is cancelled
/// </summary>
public class SaleCancelledMessage
{
    /// <summary>
    /// Sale ID
    /// </summary>
    public Guid SaleId { get; set; }
    /// <summary>
    /// Sale Number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Customer name
    /// </summary>
    public string Customer { get; set; } = string.Empty;
    /// <summary>
    /// Branch where the sale was made
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the cancellation occurred
    /// </summary>
    public DateTime OccurredAt { get; set; }
}