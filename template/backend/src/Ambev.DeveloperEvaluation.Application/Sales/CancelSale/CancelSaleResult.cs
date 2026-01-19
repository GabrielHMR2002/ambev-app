namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
/// <summary>
/// Result of cancelling a sale
/// </summary>
public class CancelSaleResult
{
    /// <summary>
    /// Sale ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Sale Number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Indicates if the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
    /// <summary>
    /// Date and time when the sale was updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}