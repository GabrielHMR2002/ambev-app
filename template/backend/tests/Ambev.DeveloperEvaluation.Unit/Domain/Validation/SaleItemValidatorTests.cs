// Domain/Validation/SaleItemValidatorTests.cs
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTests()
    {
        _validator = new SaleItemValidator();
    }

    [Fact(DisplayName = "Given valid item When validating Then should pass")]
    public void Validate_ValidItem_ShouldPass()
    {
        // Arrange
        var item = new SaleItem
        {
            Product = "Beer",
            Quantity = 5,
            UnitPrice = 10m,
            Discount = 0.10m
        };

        // Act
        var result = _validator.Validate(item);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Given quantity 0 When validating Then should fail")]
    public void Validate_ZeroQuantity_ShouldFail()
    {
        // Arrange
        var item = new SaleItem
        {
            Product = "Beer",
            Quantity = 0,
            UnitPrice = 10m
        };

        // Act
        var result = _validator.Validate(item);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("greater than 0"));
    }

    [Fact(DisplayName = "Given quantity above 20 When validating Then should fail")]
    public void Validate_QuantityAbove20_ShouldFail()
    {
        // Arrange
        var item = new SaleItem
        {
            Product = "Beer",
            Quantity = 25,
            UnitPrice = 10m
        };

        // Act
        var result = _validator.Validate(item);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("more than 20"));
    }

    [Fact(DisplayName = "Given discount without minimum quantity When validating Then should fail")]
    public void Validate_DiscountWithoutMinQuantity_ShouldFail()
    {
        // Arrange
        var item = new SaleItem
        {
            Product = "Beer",
            Quantity = 3,
            UnitPrice = 10m,
            Discount = 0.10m
        };

        // Act
        var result = _validator.Validate(item);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("below 4 items cannot have a discount"));
    }

    [Theory(DisplayName = "Given boundary quantities When validating Then should validate correctly")]
    [InlineData(1, 0.00, true)]   // Valid: no discount
    [InlineData(3, 0.00, true)]   // Valid: no discount
    [InlineData(3, 0.10, false)]  // Invalid: discount without minimum
    [InlineData(4, 0.10, true)]   // Valid: minimum for 10%
    [InlineData(10, 0.20, true)]  // Valid: minimum for 20%
    [InlineData(20, 0.20, true)]  // Valid: maximum
    [InlineData(21, 0.00, false)] // Invalid: above max
    public void Validate_BoundaryQuantities_ShouldValidateCorrectly(
        int quantity, decimal discount, bool expectedValid)
    {
        // Arrange
        var item = new SaleItem
        {
            Product = "Beer",
            Quantity = quantity,
            UnitPrice = 10m,
            Discount = discount
        };

        // Act
        var result = _validator.Validate(item);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }
}