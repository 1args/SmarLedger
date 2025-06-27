namespace SmartLedger.Common.Contracts.Abstractions;

/// <inheritdoc />
public record Event : IEvent
{
    /// <summary>Unique event identifier for further tracking.</summary>
    public Guid CorrelationId { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Event()
    {
        CorrelationId = Guid.NewGuid();
    }

    /// <summary>
    /// Constructor with correlation ID
    /// </summary>
    /// <param name="correlationId">Unique event identifier.</param>
    public Event(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}