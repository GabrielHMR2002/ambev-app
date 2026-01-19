using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.Interfaces;
using Ambev.DeveloperEvaluation.MessageBroker.Messages;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

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

        // =========================
        // UPDATE BASIC DATA
        // =========================
        sale.Customer = command.Customer;
        sale.Branch = command.Branch;

        // =========================
        // UPDATE ITEMS (SIMPLIFICADO)
        // Usa Product como chave
        // =========================

        // Remove itens que não vieram no request
        var itemsToRemove = sale.Items
            .Where(existing =>
                !command.Items.Any(i =>
                    i.Product.Equals(existing.Product, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            sale.Items.Remove(item);
        }

        // Atualiza ou adiciona itens
        foreach (var itemCommand in command.Items)
        {
            var item = sale.Items.FirstOrDefault(i =>
                i.Product.Equals(itemCommand.Product, StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                // Atualiza item existente
                item.Quantity = itemCommand.Quantity;
                item.UnitPrice = itemCommand.UnitPrice;
            }
            else
            {
                // Cria novo item
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

        // =========================
        // RECALCULATE TOTAL
        // =========================
        sale.CalculateTotalAmount();

        // =========================
        // VALIDATE AGGREGATE
        // =========================
        var saleValidation = sale.Validate();
        if (!saleValidation.IsValid)
        {
            throw new ValidationException(
                saleValidation.Errors.Select(e =>
                    new FluentValidation.Results.ValidationFailure(e.Error, e.Detail))
            );
        }

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // =========================
        // PUBLISH EVENT (BEST EFFORT)
        // =========================
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
