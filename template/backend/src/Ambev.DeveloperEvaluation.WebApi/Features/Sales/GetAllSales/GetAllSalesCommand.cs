using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;

public record GetAllSalesCommand : IRequest<IEnumerable<GetAllSalesResult>>;