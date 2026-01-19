namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
/// <summary>
/// Request to update a sale
/// </summary>
public class UpdateSaleRequest
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public string Customer { get; set; } = string.Empty;
    /// <summary>
    /// Branch where the sale was made
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Items included in the sale
    /// </summary>
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
}
/// <summary>
/// Request to update a sale item
/// </summary>
public class UpdateSaleItemRequest
{
    /// <summary>
    /// Item Id
    /// </summary>
    public Guid? Id { get; set; }
    /// <summary>
    /// Product Name
    /// </summary>
    public string Product { get; set; } = string.Empty;
    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// Unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
}