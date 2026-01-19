using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
/// <summary>
/// Validator for cancelling a sale
/// </summary>
public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CancelSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}