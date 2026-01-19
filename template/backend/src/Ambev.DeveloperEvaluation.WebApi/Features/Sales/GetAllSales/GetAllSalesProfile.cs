using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
/// <summary>
/// Profile for GetAllSales feature
/// </summary>
public class GetAllSalesProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public GetAllSalesProfile()
    {
        CreateMap<GetAllSalesResult, GetAllSalesResponse>();
        CreateMap<GetAllSalesItemResult, GetAllSalesItemResponse>();
    }
}