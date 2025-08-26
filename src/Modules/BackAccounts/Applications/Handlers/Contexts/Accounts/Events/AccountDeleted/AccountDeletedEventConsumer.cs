using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Events.AccountDeleted;

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