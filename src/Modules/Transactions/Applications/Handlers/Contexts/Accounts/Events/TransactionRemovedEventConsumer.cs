using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events;

/// <summary>
/// Consumes the <see cref="TransactionRemovedEvent"/>.
/// </summary>
public sealed class TransactionRemovedEventConsumer(
    AccountsSynchronizationService accountsSynchronizationService) : IEventConsumer<TransactionRemovedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionRemovedEvent @event, CancellationToken cancellationToken)
    {
        var request = new TransactionRemovalModel(
            @event.TransactionId,
            @event.AccountId);

        await accountsSynchronizationService.SynchronizeTransactionRemovalAsync(request, cancellationToken);
    }
}