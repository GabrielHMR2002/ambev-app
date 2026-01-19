namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
/// <summary>
/// Request to get a sale by ID
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public Guid Id { get; set; }
}