using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleItemHandler> _logger;

    public CancelSaleItemHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CancelSaleItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleItemValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.SaleId} not found");

        if (sale.IsCancelled)
            throw new InvalidOperationException("Cannot cancel item from a cancelled sale");

        var item = sale.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item == null)
            throw new KeyNotFoundException($"Item with ID {request.ItemId} not found in sale");

        if (item.IsCancelled)
            throw new InvalidOperationException("Item is already cancelled");

        item.Cancel();
        sale.CalculateTotalAmount();

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish event
        var itemCancelledEvent = new ItemCancelledEvent(item, sale.Id);
        _logger.LogInformation(
            "ItemCancelledEvent: Item {Product} cancelled from Sale {SaleNumber} at {OccurredAt}",
            itemCancelledEvent.Item.Product,
            sale.SaleNumber,
            itemCancelledEvent.OccurredAt);

        return new CancelSaleItemResult
        {
            SaleId = sale.Id,
            ItemId = item.Id,
            IsCancelled = item.IsCancelled,
            NewTotalAmount = sale.TotalAmount
        };
    }
}