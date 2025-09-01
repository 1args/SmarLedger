using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.BudgetDeleted;

/// <summary>
/// Consumes the <see cref="BudgetDeletedEvent"/>.
/// </summary>
public sealed class BudgetDeletedEventConsumer(
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<BudgetDeletedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(BudgetDeletedEvent @event, CancellationToken cancellationToken)
    {
        var request = new IdOnlyModel(@event.BudgetId);

        await budgetsSynchronizationService.SynchronizeBudgetDeletionAsync(request, cancellationToken);
    }
}