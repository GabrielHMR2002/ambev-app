using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for SaleItem entity
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.Product)
            .NotEmpty().WithMessage("Product is required")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");

        RuleFor(item => item.Discount)
            .InclusiveBetween(0, 1).WithMessage("Discount must be between 0 and 1");

        RuleFor(item => item)
            .Must(ValidateDiscountRules)
            .WithMessage("Discount rules violated: purchases below 4 items cannot have a discount");
    }

    private bool ValidateDiscountRules(SaleItem item)
    {
        // Purchases below 4 items cannot have a discount
        if (item.Quantity < 4 && item.Discount > 0)
        {
            return false;
        }

        return true;
    }
}