using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
/// <summary>
/// Validator for GetAllSalesRequest
/// </summary>
public class GetAllSalesRequestValidator : AbstractValidator<GetAllSalesRequest>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public GetAllSalesRequestValidator()
    {
    }
}