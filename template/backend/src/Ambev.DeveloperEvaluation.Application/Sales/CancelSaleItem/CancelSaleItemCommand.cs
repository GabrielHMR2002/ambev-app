using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
/// <summary>
/// Command to cancel a sale item
/// </summary>
/// <param name="SaleId"></param>
/// <param name="ItemId"></param>
public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest<CancelSaleItemResult>;