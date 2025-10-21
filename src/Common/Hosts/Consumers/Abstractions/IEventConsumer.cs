using MassTransit;
using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Host.Consumers;

/// <summary>
/// Event consumer.
/// </summary>
/// <typeparam name="TEvent">Event type.</typeparam>
public interface IEventConsumer<in TEvent> : IConsumer<TEvent>
    where TEvent : class, IEvent
{
    /// <summary>
    /// Consumes the event.
    /// </summary>
    /// <param name="event">Event.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ConsumeAsync(TEvent @event, CancellationToken cancellationToken);

    /// <inheritdoc/>
    async Task IConsumer<TEvent>.Consume(ConsumeContext<TEvent> context)
    {
        await ConsumeAsync(context.Message, context.CancellationToken);
    }
}