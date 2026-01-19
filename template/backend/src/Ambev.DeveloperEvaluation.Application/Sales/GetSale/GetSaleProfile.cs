using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;
/// <summary>
/// Profile for getting a sale by Id
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
        
        CreateMap<Sale, GetSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<SaleItem, GetSaleItemResult>();
    }
}