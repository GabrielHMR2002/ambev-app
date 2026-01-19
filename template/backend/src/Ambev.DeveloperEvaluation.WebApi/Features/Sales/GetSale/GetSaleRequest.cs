namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
/// <summary>
/// Request to get a sale by ID
/// </summary>
public class GetSaleRequest
{
    public Guid Id { get; set; }
}