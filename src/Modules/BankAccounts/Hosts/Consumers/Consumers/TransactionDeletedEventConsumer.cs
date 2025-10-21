using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Hosts.Consumers;

/// <summary>
/// Consumes the <see cref="TransactionDeletedEvent"/>.
/// </summary>
public sealed class TransactionDeletedEventConsumer(
    IAccountsSynchronizationService accountsSynchronizationService,
    IDateTimeProvider dateTimeProvider) : IEventConsumer<TransactionDeletedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionDeletedEvent @event, CancellationToken cancellationToken)
    {
        var request = new TransactionDeletionSynchronizationModel(
            @event.TransactionId,
            dateTimeProvider.UtcNow);

        await accountsSynchronizationService.SynchronizeTransactionDeletionAsync(request, cancellationToken);
    }
}