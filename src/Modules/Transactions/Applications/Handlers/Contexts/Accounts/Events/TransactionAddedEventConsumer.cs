using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events;

public sealed class TransactionAddedEventConsumer : IEventConsumer<TransactionAddedEvent>
{
    public Task ConsumeAsync(TransactionAddedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Got TransactionAddedEvent {@event.TransactionId}");
        return Task.CompletedTask; ;
    }
}