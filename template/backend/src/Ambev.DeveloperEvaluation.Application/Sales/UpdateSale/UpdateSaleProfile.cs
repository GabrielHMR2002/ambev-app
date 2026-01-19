using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
/// <summary>
/// Profile for updating a sale
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleItemCommand, SaleItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()));
        
        CreateMap<Sale, UpdateSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<SaleItem, UpdateSaleItemResult>();
    }
}