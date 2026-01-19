// Application/Sales/GetSaleHandlerTests.cs
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given existing sale When getting Then should return sale data")]
    public async Task Handle_ExistingSale_ShouldReturnSaleData()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);
        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE001",
            Customer = "Customer A"
        };

        var result = new GetSaleResult
        {
            Id = saleId,
            SaleNumber = "SALE001"
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // Act
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        getSaleResult.Should().NotBeNull();
        getSaleResult.Id.Should().Be(saleId);
    }

    [Fact(DisplayName = "Given non-existent sale When getting Then should throw KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact(DisplayName = "Given empty ID When getting Then should throw ValidationException")]
    public async Task Handle_EmptyId_ShouldThrowValidationException()
    {
        // Arrange
        var command = new GetSaleCommand(Guid.Empty);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}