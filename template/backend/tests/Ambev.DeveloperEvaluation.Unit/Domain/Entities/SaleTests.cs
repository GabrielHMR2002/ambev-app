using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Testes para a entidade Sale (Agregado)
/// </summary>
public class SaleTests
{
    #region Total Amount Calculation Tests

    [Fact(DisplayName = "Given sale with multiple items When calculating total Then should sum all item totals")]
    public void CalculateTotalAmount_MultipleItems_ShouldSumAllItemTotals()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Beer A",
                    Quantity = 5,
                    UnitPrice = 10m,
                    Discount = 0.10m,
                    TotalAmount = 45m
                },
                new()
                {
                    Product = "Beer B",
                    Quantity = 10,
                    UnitPrice = 15m,
                    Discount = 0.20m,
                    TotalAmount = 120m
                }
            }
        };

        // Act
        sale.CalculateTotalAmount();

        // Assert
        sale.TotalAmount.Should().Be(165m); 
    }

    [Fact(DisplayName = "Given sale with cancelled items When calculating total Then should exclude cancelled items")]
    public void CalculateTotalAmount_WithCancelledItems_ShouldExcludeCancelledItems()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Beer A",
                    Quantity = 5,
                    UnitPrice = 10m,
                    TotalAmount = 50m,
                    IsCancelled = false
                },
                new()
                {
                    Product = "Beer B",
                    Quantity = 10,
                    UnitPrice = 15m,
                    TotalAmount = 150m,
                    IsCancelled = true 
                }
            }
        };

        // Act
        sale.CalculateTotalAmount();

        // Assert
        sale.TotalAmount.Should().Be(50m); 
    }

    [Fact(DisplayName = "Given sale with no items When calculating total Then total should be 0")]
    public void CalculateTotalAmount_NoItems_ShouldBeZero()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>()
        };

        // Act
        sale.CalculateTotalAmount();

        // Assert
        sale.TotalAmount.Should().Be(0m);
    }

    #endregion

    #region Item Management Tests

    [Fact(DisplayName = "Given new item When adding to sale Then should include in items and recalculate total")]
    public void AddItem_NewItem_ShouldAddAndRecalculateTotal()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>()
        };

        var item = new SaleItem
        {
            Product = "Beer",
            Quantity = 5,
            UnitPrice = 10m,
            TotalAmount = 50m
        };

        // Act
        sale.AddItem(item);

        // Assert
        sale.Items.Should().Contain(item);
        sale.TotalAmount.Should().Be(50m);
    }

    [Fact(DisplayName = "Given existing item When removing from sale Then should remove and recalculate total")]
    public void RemoveItem_ExistingItem_ShouldRemoveAndRecalculateTotal()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = itemId,
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m,
                    TotalAmount = 50m
                }
            }
        };
        sale.CalculateTotalAmount();

        // Act
        sale.RemoveItem(itemId);

        // Assert
        sale.Items.Should().NotContain(i => i.Id == itemId);
        sale.TotalAmount.Should().Be(0m);
    }

    [Fact(DisplayName = "Given non-existing item When removing Then should do nothing")]
    public void RemoveItem_NonExistingItem_ShouldDoNothing()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m,
                    TotalAmount = 50m
                }
            }
        };
        sale.CalculateTotalAmount();
        var initialCount = sale.Items.Count;
        var initialTotal = sale.TotalAmount;

        // Act
        sale.RemoveItem(Guid.NewGuid());

        // Assert
        sale.Items.Should().HaveCount(initialCount);
        sale.TotalAmount.Should().Be(initialTotal);
    }

    #endregion

    #region Cancellation Tests

    [Fact(DisplayName = "Given active sale When cancelling Then should mark as cancelled and update timestamp")]
    public void Cancel_ActiveSale_ShouldMarkAsCancelledAndUpdateTimestamp()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            IsCancelled = false,
            UpdatedAt = null
        };

        // Act
        var beforeCancellation = DateTime.UtcNow;
        sale.Cancel();
        var afterCancellation = DateTime.UtcNow;

        // Assert
        sale.IsCancelled.Should().BeTrue();
        sale.UpdatedAt.Should().NotBeNull();
        sale.UpdatedAt.Should().BeOnOrAfter(beforeCancellation);
        sale.UpdatedAt.Should().BeOnOrBefore(afterCancellation);
    }

    #endregion

    #region Validation Tests

    [Fact(DisplayName = "Given valid sale When validating Then should return valid result")]
    public void Validate_ValidSale_ShouldReturnValid()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.UtcNow,
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m,
                    Discount = 0.10m
                }
            }
        };

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given sale with empty sale number When validating Then should return invalid")]
    public void Validate_EmptySaleNumber_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "",
            SaleDate = DateTime.UtcNow,
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m
                }
            }
        };

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Sale number is required"));
    }

    [Fact(DisplayName = "Given sale with empty customer When validating Then should return invalid")]
    public void Validate_EmptyCustomer_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.UtcNow,
            Customer = "",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m
                }
            }
        };

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Customer is required"));
    }

    [Fact(DisplayName = "Given sale with no items When validating Then should return invalid")]
    public void Validate_NoItems_ShouldReturnInvalid()
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
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Sale must have at least one item"));
    }

    [Fact(DisplayName = "Given sale with future date When validating Then should return invalid")]
    public void Validate_FutureDate_ShouldReturnInvalid()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            SaleDate = DateTime.UtcNow.AddDays(1),
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m
                }
            }
        };

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Detail.Contains("Sale date cannot be in the future"));
    }

    #endregion

    #region Integration Tests - Complex Scenarios

    [Fact(DisplayName = "Given sale with mixed discount tiers When calculating total Then should apply correct discounts")]
    public void CalculateTotalAmount_MixedDiscountTiers_ShouldApplyCorrectDiscounts()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            Items = new List<SaleItem>
            {
                new() // No discount
                {
                    Product = "Beer A",
                    Quantity = 3,
                    UnitPrice = 10m,
                    Discount = 0m,
                    TotalAmount = 30m
                },
                new() // 10% discount
                {
                    Product = "Beer B",
                    Quantity = 5,
                    UnitPrice = 10m,
                    Discount = 0.10m,
                    TotalAmount = 45m
                },
                new() // 20% discount
                {
                    Product = "Beer C",
                    Quantity = 15,
                    UnitPrice = 10m,
                    Discount = 0.20m,
                    TotalAmount = 120m
                }
            }
        };

        // Act
        sale.CalculateTotalAmount();

        // Assert
        sale.TotalAmount.Should().Be(195m); // 30 + 45 + 120
    }

    #endregion
}