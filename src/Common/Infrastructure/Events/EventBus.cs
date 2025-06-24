using MassTransit;
using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;

namespace SmartLedger.Common.Infrastructure.Events;

public sealed class EventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        return publishEndpoint.Publish(@event, cancellationToken);
    }
}