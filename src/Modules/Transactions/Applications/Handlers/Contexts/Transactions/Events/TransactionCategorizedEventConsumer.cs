using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Events;

public sealed class TransactionCategorizedEventConsumer : IEventConsumer<TransactionCategorizedEvent>
{
    public Task ConsumeAsync(TransactionCategorizedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Got TransactionCategorizedEvent {@event.TransactionId}");
        return Task.CompletedTask;
    }
}