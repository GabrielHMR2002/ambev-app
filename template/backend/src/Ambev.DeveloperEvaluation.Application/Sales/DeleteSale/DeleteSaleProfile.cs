using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
/// <summary>
/// Profile for deleting a sale
/// </summary>
public class DeleteSaleProfile : Profile
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DeleteSaleProfile()
    {
        CreateMap<Guid, DeleteSaleCommand>()
            .ConstructUsing(id => new DeleteSaleCommand(id));
    }
}