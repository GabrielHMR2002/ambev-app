namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
/// <summary>
/// Response for CreateSale feature
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public Guid Id { get; set; }
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
    /// Total Amount
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Branch
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Indicates if the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
    /// <summary>
    /// Items
    /// </summary>
    public List<CreateSaleItemResponse> Items { get; set; } = new();
}
/// <summary>
/// Response to create a sale item
/// </summary>
public class CreateSaleItemResponse
{
    /// <summary>
    /// Item Id
    /// </summary>
    public Guid Id { get; set; }
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
    /// <summary>
    /// Discount
    /// </summary>
    public decimal Discount { get; set; }
    /// <summary>
    /// Total Price
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Indicates if the item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}