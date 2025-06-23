using MassTransit;
using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Domain.Events;

namespace SmartLedger.Common.Applications.Handlers.Abstractions;

/// <summary>
/// Event consumer.
/// </summary>
/// <typeparam name="TEvent">Event type.</typeparam>
public interface IEventConsumer<in TEvent> : IConsumer<TEvent>
    where TEvent : class, IMessage
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

/// <summary>
/// Domain event consumer.
/// </summary>
/// <typeparam name="TEvent">Event type.</typeparam>
public interface IDomainEventConsumer<in TEvent> : IEventConsumer<TEvent> 
    where TEvent : class, IDomainEvent;

/// <summary>
/// Integration event consumer.
/// </summary>
/// <typeparam name="TEvent">Event type.</typeparam>
public interface IIntegrationEventConsumer<in TEvent> : IEventConsumer<TEvent>
    where TEvent : class, IIntegrationEvent;