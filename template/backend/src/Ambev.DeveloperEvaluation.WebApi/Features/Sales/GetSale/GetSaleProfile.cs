using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
/// <summary>
/// Profile for GetSale feature
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Guid, GetSaleCommand>()
            .ConstructUsing(id => new GetSaleCommand(id));
        
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();
    }
}