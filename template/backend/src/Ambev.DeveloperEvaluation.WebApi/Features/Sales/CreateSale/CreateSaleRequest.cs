namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
/// <summary>
/// Request to create a sale
/// </summary>
public class CreateSaleRequest
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}
/// <summary>
/// Request to create a sale item
/// </summary>
public class CreateSaleItemRequest
{
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}