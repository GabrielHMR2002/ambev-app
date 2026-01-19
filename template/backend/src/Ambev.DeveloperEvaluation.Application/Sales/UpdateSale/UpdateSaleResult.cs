namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
/// <summary>
/// Result of updating a sale
/// </summary>
public class UpdateSaleResult
{
    /// <summary>
    /// Id of the sale
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
    public DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// Items included in the sale
    /// </summary>
    public List<UpdateSaleItemResult> Items { get; set; } = new();
}
/// <summary>
/// Result for a sale item in updating a sale
/// </summary>
public class UpdateSaleItemResult
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
    /// Discount on the product
    /// </summary>
    public decimal Discount { get; set; }
    /// <summary>
    /// Total amount for the item
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Indicates if the sale item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}