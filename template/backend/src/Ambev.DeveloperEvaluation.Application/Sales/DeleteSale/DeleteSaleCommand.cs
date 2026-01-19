using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
/// <summary>
/// Command to delete a sale
/// </summary>
/// <param name="Id"></param>
public record DeleteSaleCommand(Guid Id) : IRequest<DeleteSaleResult>;