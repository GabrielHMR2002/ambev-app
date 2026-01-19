namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
/// <summary>
/// Response for GetAllSales feature
/// </summary>
public class GetAllSalesResponse
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
    /// Date of the Sale
    /// </summary>
    public DateTime SaleDate { get; set; }
    /// <summary>
    /// Customer Name
    /// </summary>
    public string Customer { get; set; } = string.Empty;
    /// <summary>
    /// Total Amount of the Sale
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Branch where the sale was made
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Indicates if the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
    /// <summary>
    /// Date and time when the sale was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Items included in the sale
    /// </summary>
    public List<GetAllSalesItemResponse> Items { get; set; } = new();
}
/// <summary>
/// Response to get all sales item
/// </summary>
public class GetAllSalesItemResponse
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
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// Unit Price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// Discount applied to the item
    /// </summary>
    public decimal Discount { get; set; }
    /// <summary>
    /// Total Price of the item
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Indicates if the item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}