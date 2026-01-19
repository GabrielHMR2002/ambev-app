using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;
/// <summary>
/// Validator for getting a sale by Id
/// </summary>
public class GetSaleValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public GetSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}