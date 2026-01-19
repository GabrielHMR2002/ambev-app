namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
/// <summary>
/// Result of getting all sales
/// </summary>
public class GetAllSalesResult
{
    /// <summary>
    /// Unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Sale Number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Date of the sale
    /// </summary>
    public DateTime SaleDate { get; set; }
    /// <summary>
    /// Customer name
    /// </summary>
    public string Customer { get; set; } = string.Empty;
    /// <summary>
    /// Total amount of the sale
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
    public List<GetAllSalesItemResult> Items { get; set; } = new();
}
/// <summary>
/// Result of a sale item in getting all sales
/// </summary>
public class GetAllSalesItemResult
{
    /// <summary>
    /// Unique identifier of the sale item
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Product name
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
    /// <summary>
    /// Discount applied to the product
    /// </summary>
    public decimal Discount { get; set; }
    /// <summary>
    /// Total amount for the item
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Indicates if the item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}