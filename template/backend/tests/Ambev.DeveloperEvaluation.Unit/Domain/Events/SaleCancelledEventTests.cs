using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Events;

public class SaleCancelledEventTests
{
    [Fact(DisplayName = "Given cancelled sale When creating event Then should contain sale data")]
    public void Constructor_CancelledSale_ShouldContainSaleData()
    {
        // Arrange
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "SALE001",
            IsCancelled = true
        };

        // Act
        var @event = new SaleCancelledEvent(sale);

        // Assert
        @event.Sale.Should().Be(sale);
        @event.Sale.IsCancelled.Should().BeTrue();
        @event.OccurredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}