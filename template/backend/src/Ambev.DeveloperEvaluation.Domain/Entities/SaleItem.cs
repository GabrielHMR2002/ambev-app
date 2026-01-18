using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale identifier this item belongs to.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product name/identifier.
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of products.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to this item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (after discount).
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Navigation property to the sale.
    /// </summary>
    public Sale? Sale { get; set; }

    /// <summary>
    /// Performs validation of the sale item entity.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Applies discount based on quantity rules and calculates total amount.
    /// Business Rules:
    /// - Purchases above 4 identical items have a 10% discount
    /// - Purchases between 10 and 20 identical items have a 20% discount
    /// - It's not possible to sell above 20 identical items
    /// - Purchases below 4 items cannot have a discount
    /// </summary>
    public void ApplyDiscount()
    {
        if (Quantity < 4)
        {
            Discount = 0;
        }
        else if (Quantity >= 4 && Quantity < 10)
        {
            Discount = 0.10m; // 10%
        }
        else if (Quantity >= 10 && Quantity <= 20)
        {
            Discount = 0.20m; // 20%
        }
        else
        {
            throw new InvalidOperationException("Cannot sell more than 20 identical items");
        }

        CalculateTotalAmount();
    }

    /// <summary>
    /// Calculates the total amount for this item.
    /// </summary>
    public void CalculateTotalAmount()
    {
        var subtotal = Quantity * UnitPrice;
        TotalAmount = subtotal - (subtotal * Discount);
    }

    /// <summary>
    /// Cancels this item.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
    }
}