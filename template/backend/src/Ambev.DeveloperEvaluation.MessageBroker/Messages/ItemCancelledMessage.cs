namespace Ambev.DeveloperEvaluation.MessageBroker.Messages;

/// <summary>
/// Message published when a sale item is cancelled
/// </summary>
public class ItemCancelledMessage
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OccurredAt { get; set; }
}