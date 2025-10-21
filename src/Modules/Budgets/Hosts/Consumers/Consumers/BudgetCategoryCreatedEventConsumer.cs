using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Host.Consumers.Consumers;

/// <summary>
/// Consumes the <see cref="BudgetCategoryCreatedEvent"/>.
/// </summary>
public sealed class BudgetCategoryCreatedEventConsumer(
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<BudgetCategoryCreatedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(BudgetCategoryCreatedEvent @event, CancellationToken cancellationToken)
    {
        var request = new BudgetCategoryCreationSynchronizationModel(
            @event.CategoryId,
            @event.BudgetId,
            @event.Category,
            @event.Limit,
            @event.CreatedAt);

        await budgetsSynchronizationService.SynchronizeBudgetCategoryCreationAsync(request, cancellationToken);
    }
}