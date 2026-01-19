using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
/// <summary>
/// Handler for updating a sale
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
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
    private readonly ILogger<UpdateSaleHandler> _logger;
    /// <summary>
    /// Message publisher for sending messages to RabbitMQ
    /// </summary>
    private readonly IMessagePublisher _messagePublisher;
    /// <summary>
    /// Constructor for UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="messagePublisher"></param>
    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<UpdateSaleHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }
    /// <summary>
    /// Handles the update of a sale
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (sale.IsCancelled)
            throw new InvalidOperationException("Cannot update a cancelled sale");

        sale.Customer = command.Customer;
        sale.Branch = command.Branch;

        var itemsToRemove = sale.Items
            .Where(existing =>
                !command.Items.Any(i =>
                    i.Product.Equals(existing.Product, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            sale.Items.Remove(item);
        }

        foreach (var itemCommand in command.Items)
        {
            var item = sale.Items.FirstOrDefault(i =>
                i.Product.Equals(itemCommand.Product, StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                item.Quantity = itemCommand.Quantity;
                item.UnitPrice = itemCommand.UnitPrice;
            }
            else
            {
                item = new SaleItem
                {
                    Product = itemCommand.Product,
                    Quantity = itemCommand.Quantity,
                    UnitPrice = itemCommand.UnitPrice,
                    SaleId = sale.Id
                };

                sale.Items.Add(item);
            }

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

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        try
        {
            var message = new SaleModifiedMessage
            {
                SaleId = updatedSale.Id,
                SaleNumber = updatedSale.SaleNumber,
                Customer = updatedSale.Customer,
                TotalAmount = updatedSale.TotalAmount,
                Branch = updatedSale.Branch,
                OccurredAt = DateTime.UtcNow,
                Items = updatedSale.Items.Select(i => new SaleItemMessage
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
                "sale.modified",
                messageId: Guid.NewGuid().ToString(),
                correlationId: updatedSale.Id.ToString()
            );

            _logger.LogInformation(
                "SaleModifiedEvent published: Sale {SaleNumber}",
                updatedSale.SaleNumber
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to publish SaleModifiedEvent for Sale {SaleNumber}",
                updatedSale.SaleNumber
            );
        }

        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}
