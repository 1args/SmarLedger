using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Hosts.Consumers;

/// <summary>
/// Consumes the <see cref="AccountCreatedEvent"/>.
/// </summary>
public sealed class AccountCreatedEventConsumer(
    IAccountsSynchronizationService accountsSynchronizationService) : IEventConsumer<AccountCreatedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(AccountCreatedEvent @event, CancellationToken cancellationToken)
    {
        var request = new AccountCreationSynchronizationModel(
            @event.AccountId,
            @event.Name,
            @event.UserId,
            @event.CreatedAt);

        await accountsSynchronizationService.SynchronizeAccountCreationAsync(request, cancellationToken);
    }
}