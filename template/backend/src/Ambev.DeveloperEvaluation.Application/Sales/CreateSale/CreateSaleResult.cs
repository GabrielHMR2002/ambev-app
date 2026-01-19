namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
/// <summary>
/// Result of creating a sale
/// </summary>
public class CreateSaleResult
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
    /// Branch where the sale is made
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Indicates if the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
    /// <summary>
    /// Items included in the sale
    /// </summary>
    public List<CreateSaleItemResult> Items { get; set; } = new();
}
/// <summary>
/// Result of creating a sale item
/// </summary>
public class CreateSaleItemResult
{
    /// <summary>
    /// Sale Item Id
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
    /// Discount applied to the item
    /// </summary>
    public decimal Discount { get; set; }
    /// <summary>
    /// Total amount of the item
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Indicates if the item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}