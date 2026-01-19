using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Events;

public class ItemCancelledEventTests
{
    [Fact(DisplayName = "Given cancelled item When creating event Then should contain item and sale data")]
    public void Constructor_CancelledItem_ShouldContainItemAndSaleData()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var item = new SaleItem
        {
            Id = Guid.NewGuid(),
            Product = "Beer",
            Quantity = 5,
            IsCancelled = true
        };

        // Act
        var @event = new ItemCancelledEvent(item, saleId);

        // Assert
        @event.Item.Should().Be(item);
        @event.SaleId.Should().Be(saleId);
        @event.OccurredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}