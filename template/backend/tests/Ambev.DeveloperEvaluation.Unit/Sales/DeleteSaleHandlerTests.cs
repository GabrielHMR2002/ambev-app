// Application/Sales/DeleteSaleHandlerTests.cs
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly DeleteSaleHandler _handler;

    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleHandler(_saleRepository);
    }

    [Fact(DisplayName = "Given existing sale When deleting Then should delete successfully")]
    public async Task Handle_ExistingSale_ShouldDeleteSuccessfully()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand(saleId);

        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        await _saleRepository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale When deleting Then should throw KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new DeleteSaleCommand(saleId);

        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact(DisplayName = "Given empty ID When deleting Then should throw ValidationException")]
    public async Task Handle_EmptyId_ShouldThrowValidationException()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.Empty);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}