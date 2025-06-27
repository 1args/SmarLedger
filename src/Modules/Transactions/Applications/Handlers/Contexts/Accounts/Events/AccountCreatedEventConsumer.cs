using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Events.Events;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events;

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