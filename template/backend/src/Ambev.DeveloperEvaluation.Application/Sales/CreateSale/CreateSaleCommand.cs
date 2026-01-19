using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale
/// </summary>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Sale number
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
    /// Branch where the sale is made
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Items included in the sale
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; set; } = new();
}

/// <summary>
/// Command for creating a sale item
/// </summary>
public class CreateSaleItemCommand
{
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
}