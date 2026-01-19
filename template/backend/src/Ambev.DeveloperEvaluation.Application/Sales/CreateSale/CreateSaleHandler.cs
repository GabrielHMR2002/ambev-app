using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Check if sale number already exists
        var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (existingSale != null)
            throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists");

        var sale = _mapper.Map<Sale>(command);

        // Apply business rules for each item
        foreach (var item in sale.Items)
        {
            try
            {
                item.ApplyDiscount();
            }
            catch (InvalidOperationException ex)
            {
                throw new ValidationException($"Item '{item.Product}': {ex.Message}");
            }
        }

        // Calculate total amount
        sale.CalculateTotalAmount();

        // Validate the complete sale entity
        var saleValidation = sale.Validate();
        if (!saleValidation.IsValid)
        {
            throw new ValidationException(
                saleValidation.Errors.Select(e =>
                    new FluentValidation.Results.ValidationFailure(e.Error, e.Detail))
            );
        }

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Publish SaleCreated event to RabbitMQ
        try
        {
            var message = new SaleCreatedMessage
            {
                SaleId = createdSale.Id,
                SaleNumber = createdSale.SaleNumber,
                SaleDate = createdSale.SaleDate,
                Customer = createdSale.Customer,
                TotalAmount = createdSale.TotalAmount,
                Branch = createdSale.Branch,
                OccurredAt = DateTime.UtcNow,
                Items = createdSale.Items.Select(i => new SaleItemMessage
                {
                    ItemId = i.Id,
                    Product = i.Product,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    TotalAmount = i.TotalAmount,
                    IsCancelled = i.IsCancelled
                }).ToList()
            };

            await _messagePublisher.PublishAsync(
                message,
                "sale.created",
                messageId: Guid.NewGuid().ToString(),
                correlationId: createdSale.Id.ToString()
            );

            _logger.LogInformation(
                "SaleCreatedEvent published to RabbitMQ: Sale {SaleNumber} created with total amount {TotalAmount}",
                createdSale.SaleNumber,
                createdSale.TotalAmount
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish SaleCreatedEvent for Sale {SaleNumber}", createdSale.SaleNumber);
            // Don't throw - the sale was created successfully
        }

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}