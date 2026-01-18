using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

public class GetAllSalesValidator : AbstractValidator<GetAllSalesCommand>
{
    public GetAllSalesValidator()
    {
        // No validation rules needed for GetAllSales command
        // This validator is included for consistency with the pattern
    }
}