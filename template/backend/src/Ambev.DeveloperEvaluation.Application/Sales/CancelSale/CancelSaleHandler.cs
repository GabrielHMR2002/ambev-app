using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CancelSaleHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        if (sale.IsCancelled)
            throw new InvalidOperationException("Sale is already cancelled");

        sale.Cancel();
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish SaleCancelled event to RabbitMQ
        try
        {
            var message = new SaleCancelledMessage
            {
                SaleId = updatedSale.Id,
                SaleNumber = updatedSale.SaleNumber,
                Customer = updatedSale.Customer,
                Branch = updatedSale.Branch,
                OccurredAt = DateTime.UtcNow
            };

            await _messagePublisher.PublishAsync(
                message,
                "sale.cancelled",
                messageId: Guid.NewGuid().ToString(),
                correlationId: updatedSale.Id.ToString()
            );

            _logger.LogInformation(
                "SaleCancelledEvent published to RabbitMQ: Sale {SaleNumber} cancelled",
                updatedSale.SaleNumber
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish SaleCancelledEvent for Sale {SaleNumber}", updatedSale.SaleNumber);
        }

        return _mapper.Map<CancelSaleResult>(updatedSale);
    }
}