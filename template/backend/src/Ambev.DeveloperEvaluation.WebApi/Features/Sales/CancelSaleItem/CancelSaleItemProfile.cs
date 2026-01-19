using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
/// <summary>
/// Profile for CancelSaleItem feature
/// </summary>
public class CancelSaleItemProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CancelSaleItemProfile()
    {
        CreateMap<CancelSaleItemResult, CancelSaleItemResponse>();
    }
}