using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
/// <summary>
/// Validator for deleting a sale
/// </summary>
public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DeleteSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}