using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Contracts.Events;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Events;

/// <summary>
/// Consumes the <see cref="TransactionCategorizedEvent"/>.
/// </summary>
public sealed class TransactionCategorizedEventConsumer(
    ITransactionsSynchronizationService transactionsSynchronisationService) 
    : IEventConsumer<TransactionCategorizedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionCategorizedEvent @event, CancellationToken cancellationToken)
    {
        var request = new CategorizeTransactionModel(
            @event.TransactionId,
            @event.NewCategory,
            @event.UpdatedAt);

        await transactionsSynchronisationService.SynchronizeCategoryChangeAsync(request, cancellationToken);
    }
}