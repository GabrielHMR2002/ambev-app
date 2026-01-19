// Application/Sales/CancelSaleItemHandlerTests.cs
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CancelSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleItemHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly CancelSaleItemHandler _handler;

    public CancelSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleItemHandler>>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new CancelSaleItemHandler(_saleRepository, _mapper, _logger, _messagePublisher);
    }

    [Fact(DisplayName = "Given valid item When cancelling Then should mark item as cancelled and recalculate total")]
    public async Task Handle_ValidItem_ShouldCancelItemAndRecalculateTotal()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var command = new CancelSaleItemCommand(saleId, itemId);

        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            IsCancelled = false,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = itemId,
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m,
                    TotalAmount = 50m,
                    IsCancelled = false
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Product = "Soda",
                    Quantity = 3,
                    UnitPrice = 5m,
                    TotalAmount = 15m,
                    IsCancelled = false
                }
            }
        };
        sale.CalculateTotalAmount();

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var cancelledItem = sale.Items.First(i => i.Id == itemId);
        cancelledItem.IsCancelled.Should().BeTrue();
        result.IsCancelled.Should().BeTrue();
        result.NewTotalAmount.Should().Be(15m); // Only the non-cancelled item
    }

    [Fact(DisplayName = "Given non-existent sale When cancelling item Then should throw KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var command = new CancelSaleItemCommand(saleId, itemId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {saleId} not found");
    }

    [Fact(DisplayName = "Given cancelled sale When cancelling item Then should throw InvalidOperationException")]
    public async Task Handle_CancelledSale_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var command = new CancelSaleItemCommand(saleId, itemId);

        var sale = new Sale
        {
            Id = saleId,
            IsCancelled = true
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot cancel item from a cancelled sale");
    }

    [Fact(DisplayName = "Given non-existent item When cancelling Then should throw KeyNotFoundException")]
    public async Task Handle_NonExistentItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var command = new CancelSaleItemCommand(saleId, itemId);

        var sale = new Sale
        {
            Id = saleId,
            IsCancelled = false,
            Items = new List<SaleItem>()
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Item with ID {itemId} not found in sale");
    }

    [Fact(DisplayName = "Given already cancelled item When cancelling Then should throw InvalidOperationException")]
    public async Task Handle_AlreadyCancelledItem_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var command = new CancelSaleItemCommand(saleId, itemId);

        var sale = new Sale
        {
            Id = saleId,
            IsCancelled = false,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = itemId,
                    Product = "Beer",
                    IsCancelled = true
                }
            }
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Item is already cancelled");
    }

    [Fact(DisplayName = "Given valid item cancellation When processing Then should publish ItemCancelled event")]
    public async Task Handle_ValidCancellation_ShouldPublishItemCancelledEvent()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var command = new CancelSaleItemCommand(saleId, itemId);

        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            IsCancelled = false,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = itemId,
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m,
                    TotalAmount = 50m,
                    IsCancelled = false
                }
            }
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _messagePublisher.Received(1).PublishAsync(
            Arg.Is<ItemCancelledMessage>(m =>
                m.SaleId == saleId &&
                m.ItemId == itemId &&
                m.Product == "Beer" &&
                m.Quantity == 5),
            "sale.item.cancelled",
            Arg.Any<string>(),
            Arg.Any<string>());
    }

    [Fact(DisplayName = "Given multiple items When cancelling one Then should only affect that item")]
    public async Task Handle_MultipleItems_ShouldOnlyAffectCancelledItem()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var otherItemId = Guid.NewGuid();
        var command = new CancelSaleItemCommand(saleId, itemId);

        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            IsCancelled = false,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = itemId,
                    Product = "Beer",
                    Quantity = 5,
                    UnitPrice = 10m,
                    TotalAmount = 50m,
                    IsCancelled = false
                },
                new()
                {
                    Id = otherItemId,
                    Product = "Soda",
                    Quantity = 3,
                    UnitPrice = 5m,
                    TotalAmount = 15m,
                    IsCancelled = false
                }
            }
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Items.First(i => i.Id == itemId).IsCancelled.Should().BeTrue();
        sale.Items.First(i => i.Id == otherItemId).IsCancelled.Should().BeFalse();
    }

    [Fact(DisplayName = "Given empty IDs When cancelling item Then should throw ValidationException")]
    public async Task Handle_EmptyIds_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CancelSaleItemCommand(Guid.Empty, Guid.Empty);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}