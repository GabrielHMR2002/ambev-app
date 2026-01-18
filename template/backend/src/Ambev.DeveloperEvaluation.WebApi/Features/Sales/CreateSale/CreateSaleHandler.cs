using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
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

        // Publish event (log for now)
        var saleCreatedEvent = new SaleCreatedEvent(createdSale);
        _logger.LogInformation(
            "SaleCreatedEvent: Sale {SaleNumber} created at {OccurredAt} with total amount {TotalAmount}",
            saleCreatedEvent.Sale.SaleNumber,
            saleCreatedEvent.OccurredAt,
            saleCreatedEvent.Sale.TotalAmount);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}