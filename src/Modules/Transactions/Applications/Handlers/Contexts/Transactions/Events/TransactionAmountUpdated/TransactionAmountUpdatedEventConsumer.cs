using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Events.TransactionAmountUpdated;

/// <summary>
/// Consumes the <see cref="TransactionAmountUpdatedEvent"/>.
/// </summary>
public sealed class TransactionAmountUpdatedEventConsumer(
    ITransactionsSynchronizationService transactionsSynchronisationService)
    : IEventConsumer<TransactionAmountUpdatedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionAmountUpdatedEvent @event, CancellationToken cancellationToken)
    {
        var request = new UpdateAmountModel(
            @event.TransactionId,
            @event.NewAmount,
            @event.UpdatedAt);

        await transactionsSynchronisationService.SynchronizeAmountChangeAsync(request, cancellationToken);
    }
}