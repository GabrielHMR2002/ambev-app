using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
/// <summary>
/// Command to update a sale
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Sale Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Customer name
    /// </summary>
    public string Customer { get; set; } = string.Empty;
    /// <summary>
    /// Branch name
    /// </summary>
    public string Branch { get; set; } = string.Empty;
    /// <summary>
    /// Items in the sale
    /// </summary>
    public List<UpdateSaleItemCommand> Items { get; set; } = new();
}
/// <summary>
/// Command to update a sale item
/// </summary>
public class UpdateSaleItemCommand
{
    /// <summary>
    /// Sale Item Id
    /// </summary>
    public Guid? Id { get; set; }
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