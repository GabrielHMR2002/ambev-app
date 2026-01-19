namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
/// <summary>
/// Response for CancelSale feature
/// </summary>
public class CancelSaleResponse
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
    /// Indicates if the sale is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
    /// <summary>
    /// Date and time when the sale was updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}