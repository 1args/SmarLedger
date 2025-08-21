using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Infrastructures.DataAccess.Abstractions;

/// <summary>
/// Abstraction for event bus used to publish domain or integration events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes a domain or integration event.
    /// </summary>
    /// <typeparam name="TEvent">Event type.</typeparam>
    /// <param name="event">Event.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, IEvent;
}