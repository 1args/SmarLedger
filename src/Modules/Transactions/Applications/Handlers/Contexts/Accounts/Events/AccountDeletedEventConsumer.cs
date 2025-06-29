using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Events.Events;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events;

/// <summary>
/// Consumes the <see cref="AccountDeletedEvent"/>.
/// </summary>
public sealed class AccountDeletedEventConsumer(
    IAccountsSynchronizationService accountsSynchronizationService) : IEventConsumer<AccountDeletedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(AccountDeletedEvent @event, CancellationToken cancellationToken)
    {
        var request = new IdOnlyModel(@event.AccountId);

        await accountsSynchronizationService.SynchronizeAccountDeletionAsync(request, cancellationToken);
    }
}