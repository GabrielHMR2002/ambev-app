using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Events;

public class SaleCreatedEventTests
{
    [Fact(DisplayName = "Given sale When creating event Then should contain sale data")]
    public void Constructor_ValidSale_ShouldContainSaleData()
    {
        // Arrange
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "SALE001",
            Customer = "Customer A"
        };

        // Act
        var @event = new SaleCreatedEvent(sale);

        // Assert
        @event.Sale.Should().Be(sale);
        @event.OccurredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = "Given event When created Then occurred time should be set")]
    public void Constructor_AnyEvent_ShouldSetOccurredTime()
    {
        // Arrange
        var sale = new Sale { Id = Guid.NewGuid() };
        var beforeCreation = DateTime.UtcNow;

        // Act
        var @event = new SaleCreatedEvent(sale);
        var afterCreation = DateTime.UtcNow;

        // Assert
        @event.OccurredAt.Should().BeOnOrAfter(beforeCreation);
        @event.OccurredAt.Should().BeOnOrBefore(afterCreation);
    }
}