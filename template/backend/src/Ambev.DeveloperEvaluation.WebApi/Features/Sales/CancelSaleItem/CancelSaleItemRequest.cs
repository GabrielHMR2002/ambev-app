namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
/// <summary>
/// Request to cancel a sale item
/// </summary>
public class CancelSaleItemRequest
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
}