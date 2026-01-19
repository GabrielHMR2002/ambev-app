namespace Ambev.DeveloperEvaluation.MessageBroker.Messages;

/// <summary>
/// Represents a sale item in messages
/// </summary>
public class SaleItemMessage
{
    /// <summary>
    /// Sale ID
    /// </summary>
    public Guid ItemId { get; set; }
    /// <summary>
    /// Product name
    /// </summary>
    public string Product { get; set; } = string.Empty;
    /// <summary>
    /// Quantity of the item
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// Unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// Discount applied to the item
    /// </summary>
    public decimal Discount { get; set; }
    /// <summary>
    /// Total amount of the item
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Indicates if the item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}