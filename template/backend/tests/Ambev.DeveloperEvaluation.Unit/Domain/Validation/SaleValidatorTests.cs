using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SaleValidatorTests
{
    private readonly SaleValidator _validator;

    public SaleValidatorTests()
    {
        _validator = new SaleValidator();
    }

    [Fact(DisplayName = "Given empty sale number When validating Then should fail")]
    public void Validate_EmptySaleNumber_ShouldFail()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "",
            SaleDate = DateTime.UtcNow,
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem> { new() { Product = "Beer", Quantity = 5, UnitPrice = 10m } }
        };

        // Act
        var result = _validator.Validate(sale);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SaleNumber");
    }

    [Fact(DisplayName = "Given future date When validating Then should fail")]
    public void Validate_FutureDate_ShouldFail()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.UtcNow.AddDays(1),
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem> { new() { Product = "Beer", Quantity = 5, UnitPrice = 10m } }
        };

        // Act
        var result = _validator.Validate(sale);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("cannot be in the future"));
    }

    [Fact(DisplayName = "Given no items When validating Then should fail")]
    public void Validate_NoItems_ShouldFail()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.UtcNow,
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>()
        };

        // Act
        var result = _validator.Validate(sale);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("at least one item"));
    }
}