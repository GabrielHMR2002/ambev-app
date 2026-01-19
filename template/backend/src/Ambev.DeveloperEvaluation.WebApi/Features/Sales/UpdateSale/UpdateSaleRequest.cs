namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
/// <summary>
/// Request to update a sale
/// </summary>
public class UpdateSaleRequest
{
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
}
/// <summary>
/// Request to update a sale item
/// </summary>
public class UpdateSaleItemRequest
{
    public Guid? Id { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}