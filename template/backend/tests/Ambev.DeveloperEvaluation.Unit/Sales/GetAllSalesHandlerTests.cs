// Application/Sales/GetAllSalesHandlerTests.cs
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetAllSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetAllSalesHandler _handler;

    public GetAllSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllSalesHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given sales exist When getting all Then should return all sales")]
    public async Task Handle_SalesExist_ShouldReturnAllSales()
    {
        // Arrange
        var command = new GetAllSalesCommand();
        var sales = new List<Sale>
        {
            new() { Id = Guid.NewGuid(), SaleNumber = "SALE001" },
            new() { Id = Guid.NewGuid(), SaleNumber = "SALE002" }
        };

        var results = new List<GetAllSalesResult>
        {
            new() { Id = sales[0].Id, SaleNumber = "SALE001" },
            new() { Id = sales[1].Id, SaleNumber = "SALE002" }
        };

        _saleRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(sales);
        _mapper.Map<IEnumerable<GetAllSalesResult>>(sales).Returns(results);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given no sales When getting all Then should return empty list")]
    public async Task Handle_NoSales_ShouldReturnEmptyList()
    {
        // Arrange
        var command = new GetAllSalesCommand();
        var sales = new List<Sale>();

        _saleRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(sales);
        _mapper.Map<IEnumerable<GetAllSalesResult>>(sales)
            .Returns(new List<GetAllSalesResult>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}