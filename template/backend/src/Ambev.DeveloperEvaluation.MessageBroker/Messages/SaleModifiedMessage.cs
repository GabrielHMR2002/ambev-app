namespace Ambev.DeveloperEvaluation.MessageBroker.Messages;

/// <summary>
/// Message published when a sale is modified
/// </summary>
public class SaleModifiedMessage
{
    public Guid SaleId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Branch { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public List<SaleItemMessage> Items { get; set; } = new();
}