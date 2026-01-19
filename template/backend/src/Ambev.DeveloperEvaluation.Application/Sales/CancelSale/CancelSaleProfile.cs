using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
/// <summary>
/// Profile for cancelling a sale
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<Guid, CancelSaleCommand>()
            .ConstructUsing(id => new CancelSaleCommand(id));
        
        CreateMap<Sale, CancelSaleResult>();
    }
}