using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Hosts.Consumers;

/// <summary>
/// Consumes the <see cref="TransactionCreatedEvent"/>.
/// </summary>
public sealed class TransactionCreatedEventConsumer(
    IAccountsSynchronizationService accountsSynchronizationService) : IEventConsumer<TransactionCreatedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionCreatedEvent @event, CancellationToken cancellationToken)
    {
        var request = new TransactionCreationSynchronizationModel(
            @event.TransactionId,
            @event.AccountId,
            @event.Amount,
            @event.Type,
            @event.Category,
            @event.CreatedAt,
            @event.Notes);

        await accountsSynchronizationService.SynchronizeTransactionCreationAsync(request, cancellationToken);
    }
}