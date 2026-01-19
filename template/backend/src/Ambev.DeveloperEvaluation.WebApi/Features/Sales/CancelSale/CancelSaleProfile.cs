using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
/// <summary>
/// Profile for CancelSale feature
/// </summary>
public class CancelSaleProfile : Profile
{
    public CancelSaleProfile()
    {
        CreateMap<Guid, CancelSaleCommand>()
            .ConstructUsing(id => new CancelSaleCommand(id));
        
        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}