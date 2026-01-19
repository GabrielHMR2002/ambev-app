namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Event raised when a sale is cancelled
/// </summary>
public class SaleCancelledEvent
{
    /// <summary>
    /// Gets the sale that was cancelled
    /// </summary>
    public Sale Sale { get; }
    /// <summary>
    /// Gets the date and time when the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="SaleCancelledEvent"/> class.
    /// </summary>
    /// <param name="sale"></param>
    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
        OccurredAt = DateTime.UtcNow;
    }
}