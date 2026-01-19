namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
/// <summary>
/// Request to cancel a sale item
/// </summary>
public class CancelSaleItemRequest
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public Guid SaleId { get; set; }
    /// <summary>
    /// Item Id
    /// </summary>
    public Guid ItemId { get; set; }
}