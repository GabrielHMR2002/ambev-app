using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
/// <summary>
/// Validator for cancelling a sale item
/// </summary>
public class CancelSaleItemValidator : AbstractValidator<CancelSaleItemCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CancelSaleItemValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");
    }
}