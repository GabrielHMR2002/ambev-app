namespace Ambev.DeveloperEvaluation.MessageBroker.Messages;

/// <summary>
/// Represents a sale item in messages
/// </summary>
public class SaleItemMessage
{
    public Guid ItemId { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}