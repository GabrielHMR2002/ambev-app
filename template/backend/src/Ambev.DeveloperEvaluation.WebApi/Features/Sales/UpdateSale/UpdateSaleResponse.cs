namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
/// <summary>
/// Response for UpdateSale feature
/// </summary>
public class UpdateSaleResponse
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
    public DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// Items included in the sale
    /// </summary>
    public List<UpdateSaleItemResponse> Items { get; set; } = new();
}
/// <summary>
/// Response to update sale item
/// </summary>
public class UpdateSaleItemResponse
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