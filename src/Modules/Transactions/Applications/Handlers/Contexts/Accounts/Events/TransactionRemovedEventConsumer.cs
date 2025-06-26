using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events;

public sealed class TransactionRemovedEventConsumer : IEventConsumer<TransactionRemovedEvent>
{
    public Task ConsumeAsync(TransactionRemovedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Got TransactionRemovedEvent {@event.TransactionId}");
        return Task.CompletedTask;
    }
}