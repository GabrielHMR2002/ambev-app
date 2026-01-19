// Application/Sales/CancelSaleHandlerTests.cs
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
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

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new CancelSaleHandler(_saleRepository, _mapper, _logger, _messagePublisher);
    }

    [Fact(DisplayName = "Given valid sale When cancelling Then should mark as cancelled")]
    public async Task Handle_ValidSale_ShouldMarkAsCancelled()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            IsCancelled = false
        };

        var result = new CancelSaleResult
        {
            Id = saleId,
            IsCancelled = true
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CancelSaleResult>(Arg.Any<Sale>()).Returns(result);

        // Act
        var cancelResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.IsCancelled.Should().BeTrue();
        sale.UpdatedAt.Should().NotBeNull();
        cancelResult.IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "Given non-existent sale When cancelling Then should throw KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {saleId} not found");
    }

    [Fact(DisplayName = "Given already cancelled sale When cancelling Then should throw InvalidOperationException")]
    public async Task Handle_AlreadyCancelledSale_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            IsCancelled = true
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale is already cancelled");
    }

    [Fact(DisplayName = "Given valid sale When cancelling Then should publish SaleCancelled event")]
    public async Task Handle_ValidSale_ShouldPublishSaleCancelledEvent()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            Customer = "Customer A",
            Branch = "Branch 1",
            IsCancelled = false
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _messagePublisher.Received(1).PublishAsync(
            Arg.Is<SaleCancelledMessage>(m =>
                m.SaleId == saleId &&
                m.SaleNumber == "SALE001" &&
                m.Customer == "Customer A" &&
                m.Branch == "Branch 1"),
            "sale.cancelled",
            Arg.Any<string>(),
            Arg.Any<string>());
    }

    [Fact(DisplayName = "Given empty sale ID When cancelling Then should throw ValidationException")]
    public async Task Handle_EmptySaleId_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.Empty);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given valid cancellation When updating Then should update repository")]
    public async Task Handle_ValidCancellation_ShouldUpdateRepository()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            IsCancelled = false
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => s.IsCancelled == true),
            Arg.Any<CancellationToken>());
    }
}