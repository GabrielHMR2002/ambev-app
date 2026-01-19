// Domain/Entities/SaleItemTests.cs
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Testes para a entidade SaleItem focando nas regras de negócio de desconto
/// </summary>
public class SaleItemTests
{
    #region Discount Calculation Tests - Core Business Rules

    [Fact(DisplayName = "Given quantity below 4 When applying discount Then discount should be 0%")]
    public void ApplyDiscount_QuantityBelow4_ShouldHaveNoDiscount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 3,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        saleItem.Discount.Should().Be(0m);
        saleItem.TotalAmount.Should().Be(30m); // 3 * 10 = 30
    }

    [Fact(DisplayName = "Given quantity of 4 When applying discount Then discount should be 10%")]
    public void ApplyDiscount_Quantity4_ShouldHave10PercentDiscount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 4,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        saleItem.Discount.Should().Be(0.10m);
        saleItem.TotalAmount.Should().Be(36m); // 4 * 10 * 0.9 = 36
    }

    [Fact(DisplayName = "Given quantity between 4 and 9 When applying discount Then discount should be 10%")]
    public void ApplyDiscount_QuantityBetween4And9_ShouldHave10PercentDiscount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 7,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        saleItem.Discount.Should().Be(0.10m);
        saleItem.TotalAmount.Should().Be(63m); // 7 * 10 * 0.9 = 63
    }

    [Fact(DisplayName = "Given quantity of 10 When applying discount Then discount should be 20%")]
    public void ApplyDiscount_Quantity10_ShouldHave20PercentDiscount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 10,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        saleItem.Discount.Should().Be(0.20m);
        saleItem.TotalAmount.Should().Be(80m); // 10 * 10 * 0.8 = 80
    }

    [Fact(DisplayName = "Given quantity between 10 and 20 When applying discount Then discount should be 20%")]
    public void ApplyDiscount_QuantityBetween10And20_ShouldHave20PercentDiscount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 15,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        saleItem.Discount.Should().Be(0.20m);
        saleItem.TotalAmount.Should().Be(120m); // 15 * 10 * 0.8 = 120
    }

    [Fact(DisplayName = "Given quantity of 20 When applying discount Then discount should be 20%")]
    public void ApplyDiscount_Quantity20_ShouldHave20PercentDiscount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 20,
            UnitPrice = 10m
        };

        // Act
        saleItem.ApplyDiscount();

        // Assert
        saleItem.Discount.Should().Be(0.20m);
        saleItem.TotalAmount.Should().Be(160m); // 20 * 10 * 0.8 = 160
    }

    [Fact(DisplayName = "Given quantity above 20 When applying discount Then should throw exception")]
    public void ApplyDiscount_QuantityAbove20_ShouldThrowException()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 21,
            UnitPrice = 10m
        };

        // Act
        var act = () => saleItem.ApplyDiscount();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 identical items");
    }

    #endregion

    #region Total Amount Calculation Tests

    [Fact(DisplayName = "Given item with no discount When calculating total Then should multiply quantity by price")]
    public void CalculateTotalAmount_NoDiscount_ShouldMultiplyQuantityByPrice()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 3,
            UnitPrice = 15.50m,
            Discount = 0m
        };

        // Act
        saleItem.CalculateTotalAmount();

        // Assert
        saleItem.TotalAmount.Should().Be(46.50m); // 3 * 15.50
    }

    [Fact(DisplayName = "Given item with discount When calculating total Then should apply discount correctly")]
    public void CalculateTotalAmount_WithDiscount_ShouldApplyDiscountCorrectly()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 10,
            UnitPrice = 25m,
            Discount = 0.20m
        };

        // Act
        saleItem.CalculateTotalAmount();

        // Assert
        saleItem.TotalAmount.Should().Be(200m); // 10 * 25 * 0.8 = 200
    }

    #endregion

    #region Cancellation Tests

    [Fact(DisplayName = "Given active item When cancelling Then should mark as cancelled")]
    public void Cancel_ActiveItem_ShouldMarkAsCancelled()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 5,
            UnitPrice = 10m,
            IsCancelled = false
        };

        // Act
        saleItem.Cancel();

        // Assert
        saleItem.IsCancelled.Should().BeTrue();
    }

    #endregion

    #region Validation Tests

    [Fact(DisplayName = "Given valid item When validating Then should return valid result")]
    public void Validate_ValidItem_ShouldReturnValid()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 5,
            UnitPrice = 10m,
            Discount = 0.10m
        };
        saleItem.CalculateTotalAmount();

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given item with empty product When validating Then should return invalid")]
    public void Validate_EmptyProduct_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "",
            Quantity = 5,
            UnitPrice = 10m
        };

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Product is required"));
    }

    [Fact(DisplayName = "Given item with zero quantity When validating Then should return invalid")]
    public void Validate_ZeroQuantity_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 0,
            UnitPrice = 10m
        };

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Quantity must be greater than 0"));
    }

    [Fact(DisplayName = "Given item with quantity above 20 When validating Then should return invalid")]
    public void Validate_QuantityAbove20_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 25,
            UnitPrice = 10m
        };

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Cannot sell more than 20 identical items"));
    }

    [Fact(DisplayName = "Given item with discount but quantity below 4 When validating Then should return invalid")]
    public void Validate_DiscountWithQuantityBelow4_ShouldReturnInvalid()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 3,
            UnitPrice = 10m,
            Discount = 0.10m
        };

        // Act
        var result = saleItem.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("purchases below 4 items cannot have a discount"));
    }

    #endregion

    #region Edge Cases

    [Fact(DisplayName = "Given high precision prices When calculating total Then should maintain precision")]
    public void CalculateTotalAmount_HighPrecisionPrices_ShouldMaintainPrecision()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Product = "Beer",
            Quantity = 10,
            UnitPrice = 123.456m,
            Discount = 0.20m
        };

        // Act
        saleItem.CalculateTotalAmount();

        // Assert
        saleItem.TotalAmount.Should().Be(987.648m); // 10 * 123.456 * 0.8
    }

    #endregion
}