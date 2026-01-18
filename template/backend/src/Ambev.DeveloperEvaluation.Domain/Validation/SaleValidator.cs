using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Sale entity
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters");

        RuleFor(sale => sale.Customer)
            .NotEmpty().WithMessage("Customer is required")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(sale => sale.Branch)
            .NotEmpty().WithMessage("Branch is required")
            .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future");

        RuleFor(sale => sale.Items)
            .NotEmpty().WithMessage("Sale must have at least one item");

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());
    }
}