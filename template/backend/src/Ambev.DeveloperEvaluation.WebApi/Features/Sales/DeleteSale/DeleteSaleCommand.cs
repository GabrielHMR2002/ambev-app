using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

public record DeleteSaleCommand(Guid Id) : IRequest<DeleteSaleResult>;