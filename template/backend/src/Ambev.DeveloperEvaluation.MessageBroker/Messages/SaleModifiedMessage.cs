namespace Ambev.DeveloperEvaluation.MessageBroker.Messages;

/// <summary>
/// Message published when a sale is modified
/// </summary>
public class SaleModifiedMessage
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
    /// Total amount of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Branch where the sale was made
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the modification occurred
    /// </summary>
    public DateTime OccurredAt { get; set; }
    /// <summary>
    /// Items included in the sale
    /// </summary>
    public List<SaleItemMessage> Items { get; set; } = new();
}