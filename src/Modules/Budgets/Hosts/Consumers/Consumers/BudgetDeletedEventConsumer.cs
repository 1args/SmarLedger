using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Host.Consumers.Consumers;

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