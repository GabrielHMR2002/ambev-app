using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _messagePublisher = Substitute.For<IMessagePublisher>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _logger, _messagePublisher);
    }

    #region Valid Creation Tests

    [Fact(DisplayName = "Given valid sale command When creating sale Then should return success with correct data")]
    public async Task Handle_ValidCommand_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            SaleDate = command.SaleDate,
            Items = command.Items.Select(i => new SaleItem
            {
                Product = i.Product,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        var result = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(result);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var createResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(sale.Id);
        createResult.SaleNumber.Should().Be(sale.SaleNumber);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When creating sale Then should publish SaleCreated event")]
    public async Task Handle_ValidCommand_ShouldPublishSaleCreatedEvent()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = command.Items.Select(i => new SaleItem
            {
                Product = i.Product,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _messagePublisher.Received(1).PublishAsync(
            Arg.Is<SaleCreatedMessage>(m =>
                m.SaleId == sale.Id &&
                m.SaleNumber == sale.SaleNumber),
            "sale.created",
            Arg.Any<string>(),
            Arg.Any<string>());
    }

    #endregion

    #region Business Rules Tests

    [Fact(DisplayName = "Given item with 4 units When creating sale Then should apply 10% discount")]
    public async Task Handle_ItemWith4Units_ShouldApply10PercentDiscount()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateCommandWithItemQuantity(4);
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Test Product",
                    Quantity = 4,
                    UnitPrice = 100m
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Items[0].Discount.Should().Be(0.10m);
        sale.Items[0].TotalAmount.Should().Be(360m); // 4 * 100 * 0.9
    }

    [Fact(DisplayName = "Given item with 10 units When creating sale Then should apply 20% discount")]
    public async Task Handle_ItemWith10Units_ShouldApply20PercentDiscount()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateCommandWithItemQuantity(10);
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Test Product",
                    Quantity = 10,
                    UnitPrice = 100m
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Items[0].Discount.Should().Be(0.20m);
        sale.Items[0].TotalAmount.Should().Be(800m); // 10 * 100 * 0.8
    }

    [Fact(DisplayName = "Given item with 3 units When creating sale Then should have no discount")]
    public async Task Handle_ItemWith3Units_ShouldHaveNoDiscount()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateCommandWithItemQuantity(3);
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Test Product",
                    Quantity = 3,
                    UnitPrice = 100m
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Items[0].Discount.Should().Be(0m);
        sale.Items[0].TotalAmount.Should().Be(300m); // 3 * 100
    }

    [Fact(DisplayName = "Given item with 21 units When creating sale Then should throw validation exception")]
    public async Task Handle_ItemWith21Units_ShouldThrowValidationException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateCommandWithItemQuantity(21);
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Test Product",
                    Quantity = 21,
                    UnitPrice = 100m
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Cannot sell more than 20 identical items*");
    }

    #endregion

    #region Validation Tests

    [Fact(DisplayName = "Given invalid command When creating sale Then should throw validation exception")]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateInvalidCommand();

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given duplicate sale number When creating sale Then should throw invalid operation exception")]
    public async Task Handle_DuplicateSaleNumber_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new Sale { SaleNumber = command.SaleNumber };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with number {command.SaleNumber} already exists");
    }

    #endregion

    #region Total Calculation Tests

    [Fact(DisplayName = "Given sale with multiple items When creating Then should calculate correct total")]
    public async Task Handle_MultipleItems_ShouldCalculateCorrectTotal()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateCommandWithMultipleItems(3);
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new()
                {
                    Product = "Product A",
                    Quantity = 5,
                    UnitPrice = 10m,
                    Discount = 0.10m,
                    TotalAmount = 45m
                },
                new()
                {
                    Product = "Product B",
                    Quantity = 10,
                    UnitPrice = 20m,
                    Discount = 0.20m,
                    TotalAmount = 160m
                },
                new()
                {
                    Product = "Product C",
                    Quantity = 3,
                    UnitPrice = 15m,
                    Discount = 0m,
                    TotalAmount = 45m
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.TotalAmount.Should().Be(250m); // 45 + 160 + 45
    }

    #endregion

    #region Mapper Tests

    [Fact(DisplayName = "Given valid command When handling Then should map command to sale entity")]
    public async Task Handle_ValidCommand_ShouldMapCommandToSale()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            Customer = command.Customer,
            Branch = command.Branch,
            Items = command.Items.Select(i => new SaleItem
            {
                Product = i.Product,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
            c.SaleNumber == command.SaleNumber &&
            c.Customer == command.Customer &&
            c.Branch == command.Branch));
    }

    #endregion
}