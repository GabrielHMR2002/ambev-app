using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSaleCommand(Guid Id) : IRequest<GetSaleResult>;