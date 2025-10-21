using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Host.Consumers.Consumers;

/// <summary>
/// Consumes the <see cref="BudgetCreatedEvent"/>.
/// </summary>
public sealed class BudgetCategoryDeletedEventConsumer(
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<BudgetCategoryDeletedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(BudgetCategoryDeletedEvent @event, CancellationToken cancellationToken)
    {
        var request = new BudgetCategoryDeletionSynchronizationModel(@event.CategoryId);

        await budgetsSynchronizationService.SynchronizeBudgetCategoryDeletionAsync(request, cancellationToken);
    }
}