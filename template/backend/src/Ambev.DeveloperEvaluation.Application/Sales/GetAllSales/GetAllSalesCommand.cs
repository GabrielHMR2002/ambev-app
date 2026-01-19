using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
/// <summary>
/// Command to get all sales
/// </summary>
public record GetAllSalesCommand : IRequest<IEnumerable<GetAllSalesResult>>;