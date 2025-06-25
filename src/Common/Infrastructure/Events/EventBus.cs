using MassTransit;
using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;

namespace SmartLedger.Common.Infrastructure.Events;

/// <summary>
/// Event bus implementation that uses MassTransit for publishing events.
/// </summary>
/// <param name="publishEndpoint">Sends the event to the queue to all interested customers.</param>
public sealed class EventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    /// <inheritdoc />
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        return publishEndpoint.Publish(@event, cancellationToken);
    }
}