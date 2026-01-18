using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;

public class GetAllSalesRequestValidator : AbstractValidator<GetAllSalesRequest>
{
    public GetAllSalesRequestValidator()
    {
        // No validation rules needed for GetAllSales
        // This validator is included for consistency with the pattern
    }
}