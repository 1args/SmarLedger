using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Transactions.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events;

public sealed class TransactionRemovedEventConsumer(
    IBudgetsService budgetsService,
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<TransactionRemovedIntegrationEvent>
{
    public async Task ConsumeAsync(TransactionRemovedIntegrationEvent @event, CancellationToken cancellationToken)
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