namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
/// <summary>
/// Response for CancelSaleItem feature
/// </summary>
public class CancelSaleItemResponse
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public Guid SaleId { get; set; }
    /// <summary>
    /// Item Id
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