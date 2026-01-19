using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;
/// <summary>
/// Command to get a sale by Id
/// </summary>
/// <param name="Id"></param>
public record GetSaleCommand(Guid Id) : IRequest<GetSaleResult>;