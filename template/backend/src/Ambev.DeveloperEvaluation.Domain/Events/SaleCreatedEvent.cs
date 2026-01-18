using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a new sale is created
/// </summary>
public class SaleCreatedEvent
{
    public Sale Sale { get; }
    public DateTime OccurredAt { get; }

    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
        OccurredAt = DateTime.UtcNow;
    }
}