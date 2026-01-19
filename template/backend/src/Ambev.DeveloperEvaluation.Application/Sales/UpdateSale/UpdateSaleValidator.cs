using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
/// <summary>
/// Validator for updating a sale
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public UpdateSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.Customer)
            .NotEmpty().WithMessage("Customer is required")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage("Branch is required")
            .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sale must have at least one item");

        RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemValidator());
    }
}
/// <summary>
/// Validator for updating a sale item
/// </summary>
public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public UpdateSaleItemValidator()
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