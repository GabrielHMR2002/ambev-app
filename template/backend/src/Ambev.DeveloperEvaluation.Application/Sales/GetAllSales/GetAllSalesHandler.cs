using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
/// <summary>
/// Handler for getting all sales
/// </summary>
public class GetAllSalesHandler : IRequestHandler<GetAllSalesCommand, IEnumerable<GetAllSalesResult>>
{
    /// <summary>
    /// Repository for sales
    /// </summary>
    private readonly ISaleRepository _saleRepository;
    /// <summary>
    /// Mapper for object transformations
    /// </summary>
    private readonly IMapper _mapper;
    /// <summary>
    /// Constructor for GetAllSalesHandler
    /// </summary>
    /// <param name="saleRepository"></param>
    /// <param name="mapper"></param>
    public GetAllSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }
    /// <summary>
    /// Handles the GetAllSalesCommand
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<GetAllSalesResult>> Handle(GetAllSalesCommand request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<GetAllSalesResult>>(sales);
    }
}