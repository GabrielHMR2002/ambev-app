using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Events;

public class SaleModifiedEventTests
{
    [Fact(DisplayName = "Given modified sale When creating event Then should contain updated data")]
    public void Constructor_ModifiedSale_ShouldContainUpdatedData()
    {
        // Arrange
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "SALE001",
            Customer = "Updated Customer",
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var @event = new SaleModifiedEvent(sale);

        // Assert
        @event.Sale.Should().Be(sale);
        @event.Sale.UpdatedAt.Should().NotBeNull();
        @event.OccurredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}