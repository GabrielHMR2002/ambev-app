using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
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

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
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

        // Update basic properties
        sale.Customer = command.Customer;
        sale.Branch = command.Branch;

        // Update items
        sale.Items.Clear();
        foreach (var itemCommand in command.Items)
        {
            var item = _mapper.Map<SaleItem>(itemCommand);
            item.SaleId = sale.Id;

            try
            {
                item.ApplyDiscount();
            }
            catch (InvalidOperationException ex)
            {
                throw new ValidationException($"Item '{item.Product}': {ex.Message}");
            }

            sale.Items.Add(item);
        }

        // Recalculate total
        sale.CalculateTotalAmount();

        // Validate
        var saleValidation = sale.Validate();
        if (!saleValidation.IsValid)
        {
            throw new ValidationException(
                saleValidation.Errors.Select(e =>
                    new FluentValidation.Results.ValidationFailure(e.Error, e.Detail))
            );
        }

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish event
        var saleModifiedEvent = new SaleModifiedEvent(updatedSale);
        _logger.LogInformation(
            "SaleModifiedEvent: Sale {SaleNumber} modified at {OccurredAt} with new total amount {TotalAmount}",
            saleModifiedEvent.Sale.SaleNumber,
            saleModifiedEvent.OccurredAt,
            saleModifiedEvent.Sale.TotalAmount);

        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}