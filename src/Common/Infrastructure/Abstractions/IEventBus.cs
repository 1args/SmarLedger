using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Infrastructure.Abstractions;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, IEvent;
}