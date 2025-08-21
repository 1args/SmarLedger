using MassTransit;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using Event = SmartLedger.Common.Contracts.Abstractions.Event;

namespace SmartLedger.Common.Infrastructures.DataAccess.Events;

/// <summary>
/// Event bus implementation that uses MassTransit for publishing events.
/// </summary>
/// <param name="publishEndpoint">Sends the event to the queue to all interested customers.</param>
public sealed class EventBus(
    IPublishEndpoint publishEndpoint,
    ILogger<EventBus> logger) : IEventBus
{
    /// <inheritdoc />
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        if (@event is Event baseEvent)
        {
            logger.LogInformation(
                "Publishing event {EventType} with ID {EventId}",
                baseEvent.GetType().Name,
                baseEvent.CorrelationId);
        }

        return publishEndpoint.Publish(@event, cancellationToken);
    }
}