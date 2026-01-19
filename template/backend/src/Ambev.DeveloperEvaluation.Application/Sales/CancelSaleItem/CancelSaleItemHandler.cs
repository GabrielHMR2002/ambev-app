using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
/// <summary>
/// Handler for cancelling a sale item
/// </summary>
public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{   
    /// <summary>
    /// Repository for accessing sales data
    /// </summary>
    private readonly ISaleRepository _saleRepository;
    /// <summary>
    /// Mapper for object transformations
    /// </summary>
    private readonly IMapper _mapper;
    /// <summary>
    /// Logger for logging information and errors
    /// </summary>
    private readonly ILogger<CancelSaleItemHandler> _logger;
    /// <summary>
    /// Message publisher for sending messages to RabbitMQ
    /// </summary>
    private readonly IMessagePublisher _messagePublisher;
    /// <summary>
    /// Constructor for CancelSaleItemHandler
    /// </summary>
    /// <param name="saleRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="messagePublisher"></param>
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
    /// <summary>
    /// Handles the cancellation of a sale item
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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