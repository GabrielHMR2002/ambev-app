namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
/// <summary>
/// Request to delete a sale
/// </summary>
public class DeleteSaleRequest
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public Guid Id { get; set; }
}