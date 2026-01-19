namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
/// <summary>
/// Request to cancel a sale
/// </summary>
public class CancelSaleRequest
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public Guid Id { get; set; }
}