namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
/// <summary>
/// Request to create a sale
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Sale Number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Sale Date
    /// </summary>
    public DateTime SaleDate { get; set; }
    /// <summary>
    /// Customer Name
    /// </summary>
    public string Customer { get; set; } = string.Empty;
    /// <summary>
    /// Branch
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Items
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}
/// <summary>
/// Request to create a sale item
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Product Name
    /// </summary>
    public string Product { get; set; } = string.Empty;
    /// <summary>
    /// Quantity
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// Unit Price
    /// </summary>
    public decimal UnitPrice { get; set; }
}