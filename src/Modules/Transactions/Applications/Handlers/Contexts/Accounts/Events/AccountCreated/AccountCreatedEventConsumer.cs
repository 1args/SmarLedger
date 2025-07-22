using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events.AccountCreated;

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