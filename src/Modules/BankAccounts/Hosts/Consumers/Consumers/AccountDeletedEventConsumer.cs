using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Hosts.Consumers;

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