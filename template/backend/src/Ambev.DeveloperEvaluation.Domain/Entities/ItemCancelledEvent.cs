namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Event raised when a sale item is cancelled
/// </summary>
public class ItemCancelledEvent
{
    /// <summary>
    /// Gets the sale item that was cancelled
    /// </summary>
    public SaleItem Item { get; }
    /// <summary>
    /// Gets the sale identifier to which the item belonged
    /// </summary>
    public Guid SaleId { get; }
    /// <summary>
    /// Gets the date and time when the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="saleId"></param>
    public ItemCancelledEvent(SaleItem item, Guid saleId)
    {
        Item = item;
        SaleId = saleId;
        OccurredAt = DateTime.UtcNow;
    }
}