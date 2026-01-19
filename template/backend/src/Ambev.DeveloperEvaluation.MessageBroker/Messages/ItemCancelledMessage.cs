namespace Ambev.DeveloperEvaluation.MessageBroker.Messages;

/// <summary>
/// Message published when a sale item is cancelled
/// </summary>
public class ItemCancelledMessage
{
    /// <summary>
    /// Sale ID
    /// </summary>
    public Guid SaleId { get; set; }
    /// <summary>
    /// Item ID
    /// </summary>
    public Guid ItemId { get; set; }
    /// <summary>
    /// Sale Number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Product name
    /// </summary>
    public string Product { get; set; } = string.Empty;
    /// <summary>
    /// Quantity cancelled
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// Unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// Total amount of the cancelled item
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Date and time when the cancellation occurred
    /// </summary>
    public DateTime OccurredAt { get; set; }
}