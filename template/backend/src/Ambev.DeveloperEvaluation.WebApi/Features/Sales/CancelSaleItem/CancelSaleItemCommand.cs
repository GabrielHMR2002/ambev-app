using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest<CancelSaleItemResult>;