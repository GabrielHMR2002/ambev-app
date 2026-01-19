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
    private readonly ILogger<CreateSaleHandler> _logger;
    /// <summary>
    /// Message publisher for sending messages to RabbitMQ
    /// </summary>
    private readonly IMessagePublisher _messagePublisher;
    /// <summary>
    /// Constructor for CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="messagePublisher"></param>
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
    /// <summary>
    /// Handles the creation of a new sale
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (existingSale != null)
            throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists");

        var sale = _mapper.Map<Sale>(command);

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

        sale.CalculateTotalAmount();

        var saleValidation = sale.Validate();
        if (!saleValidation.IsValid)
        {
            throw new ValidationException(
                saleValidation.Errors.Select(e =>
                    new FluentValidation.Results.ValidationFailure(e.Error, e.Detail))
            );
        }

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

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
        }

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}