using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.BankAccounts.Contracts.Events;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Host.Consumers.Consumers;

/// <summary>
/// Consumes the <see cref="TransactionDeletedEvent"/>.
/// </summary>
public sealed class TransactionDeletedEventConsumer(
    IBudgetsService budgetsService,
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<TransactionDeletedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionDeletedEvent @event, CancellationToken cancellationToken)
    {
        var request = new TransactionModificationModel(
            @event.UserId,
            @event.Amount,
            @event.Type,
            @event.Category,
            @event.CreatedAt);

        await Task.WhenAll(
            budgetsService.RevertSpendingAmountAsync(request, cancellationToken),
            budgetsSynchronizationService.SynchronizeReversionSpendingAmountAsync(request, cancellationToken));
    }
}