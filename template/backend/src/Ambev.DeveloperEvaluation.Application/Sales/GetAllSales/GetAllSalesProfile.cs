using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
/// <summary>
/// Profile for getting all sales
/// </summary>
public class GetAllSalesProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public GetAllSalesProfile()
    {
        CreateMap<Sale, GetAllSalesResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<SaleItem, GetAllSalesItemResult>();
    }
}