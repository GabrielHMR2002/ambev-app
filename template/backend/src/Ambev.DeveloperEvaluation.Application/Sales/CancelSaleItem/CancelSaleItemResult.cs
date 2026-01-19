namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
/// <summary>
/// Result of cancelling a sale item
/// </summary>
public class CancelSaleItemResult
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
    /// Indicates if the item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
    /// <summary>
    /// New total amount of the sale after item cancellation
    /// </summary>
    public decimal NewTotalAmount { get; set; }
}