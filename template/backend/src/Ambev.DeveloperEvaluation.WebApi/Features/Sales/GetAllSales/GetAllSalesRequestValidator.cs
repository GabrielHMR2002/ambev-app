using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
/// <summary>
/// Validator for GetAllSalesRequest
/// </summary>
public class GetAllSalesRequestValidator : AbstractValidator<GetAllSalesRequest>
{
    public GetAllSalesRequestValidator()
    {
        // This validator is included for consistency with the pattern
    }
}