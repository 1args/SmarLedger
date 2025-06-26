using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Events;

public sealed class TransactionAmountUpdatedEventConsumer : IEventConsumer<TransactionAmountUpdatedEvent>
{
    public Task ConsumeAsync(TransactionAmountUpdatedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Got TransactionAmountUpdatedEventConsumer {@event.TransactionId}");
        return Task.CompletedTask;
    }
}