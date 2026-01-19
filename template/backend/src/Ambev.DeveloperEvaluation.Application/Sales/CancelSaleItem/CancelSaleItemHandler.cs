using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleItemHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public CancelSaleItemHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CancelSaleItemHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _messagePublisher = messagePublisher;
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

        // Publish ItemCancelled event to RabbitMQ
        try
        {
            var message = new ItemCancelledMessage
            {
                SaleId = sale.Id,
                ItemId = item.Id,
                SaleNumber = sale.SaleNumber,
                Product = item.Product,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalAmount = item.TotalAmount,
                OccurredAt = DateTime.UtcNow
            };

            await _messagePublisher.PublishAsync(
                message,
                "sale.item.cancelled",
                messageId: Guid.NewGuid().ToString(),
                correlationId: sale.Id.ToString()
            );

            _logger.LogInformation(
                "ItemCancelledEvent published to RabbitMQ: Item {Product} cancelled from Sale {SaleNumber}",
                item.Product,
                sale.SaleNumber
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish ItemCancelledEvent for Item {ItemId} in Sale {SaleNumber}", item.Id, sale.SaleNumber);
        }

        return new CancelSaleItemResult
        {
            SaleId = sale.Id,
            ItemId = item.Id,
            IsCancelled = item.IsCancelled,
            NewTotalAmount = sale.TotalAmount
        };
    }
}