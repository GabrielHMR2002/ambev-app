// Application/Sales/UpdateSaleHandlerTests.cs
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _logger, _messagePublisher);
    }

    [Fact(DisplayName = "Given valid update When updating sale Then should update successfully")]
    public async Task Handle_ValidUpdate_ShouldUpdateSuccessfully()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        command.Id = saleId;

        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            Customer = "Old Customer",
            Branch = "Old Branch",
            IsCancelled = false,
            Items = new List<SaleItem>()
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Customer.Should().Be(command.Customer);
        sale.Branch.Should().Be(command.Branch);
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given cancelled sale When updating Then should throw InvalidOperationException")]
    public async Task Handle_CancelledSale_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        command.Id = saleId;

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
            .WithMessage("Cannot update a cancelled sale");
    }

    [Fact(DisplayName = "Given item with 21 units When updating Then should throw ValidationException")]
    public async Task Handle_ItemWith21Units_ShouldThrowValidationException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = UpdateSaleHandlerTestData.GenerateCommandWithQuantity(21);
        command.Id = saleId;

        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            IsCancelled = false,
            Items = new List<SaleItem>()
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Cannot sell more than 20 identical items*");
    }

    [Fact(DisplayName = "Given valid update When processing Then should publish SaleModified event")]
    public async Task Handle_ValidUpdate_ShouldPublishSaleModifiedEvent()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        command.Id = saleId;

        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            Customer = "Customer",
            Branch = "Branch",
            IsCancelled = false,
            Items = new List<SaleItem>()
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _messagePublisher.Received(1).PublishAsync(
            Arg.Is<SaleModifiedMessage>(m => m.SaleId == saleId),
            "sale.modified",
            Arg.Any<string>(),
            Arg.Any<string>());
    }
}
