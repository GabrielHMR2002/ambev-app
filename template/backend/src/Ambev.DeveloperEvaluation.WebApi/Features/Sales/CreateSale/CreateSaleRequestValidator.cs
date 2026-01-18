using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters");

        RuleFor(x => x.Customer)
            .NotEmpty().WithMessage("Customer is required")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage("Branch is required")
            .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(x => x.SaleDate)
            .NotEmpty().WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sale must have at least one item");

        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemRequestValidator());
    }
}

public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    public CreateSaleItemRequestValidator()
    {
        RuleFor(x => x.Product)
            .NotEmpty().WithMessage("Product is required")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0");
    }
}