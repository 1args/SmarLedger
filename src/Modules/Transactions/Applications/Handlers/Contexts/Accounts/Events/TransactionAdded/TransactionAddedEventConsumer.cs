using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Events.TransactionAdded;

/// <summary>
/// Consumes the <see cref="TransactionAddedEvent"/>.
/// </summary>
public sealed class TransactionAddedEventConsumer(
    IAccountsSynchronizationService accountsSynchronizationService) : IEventConsumer<TransactionAddedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionAddedEvent @event, CancellationToken cancellationToken)
    {
        var request = new TransactionAdditionSynchronizationModel(
            @event.TransactionId,
            @event.AccountId,
            @event.Amount,
            @event.Type,
            @event.Category,
            @event.CreatedAt,
            @event.Notes);

        await accountsSynchronizationService.SynchronizeTransactionAdditionAsync(request, cancellationToken);
    }
}