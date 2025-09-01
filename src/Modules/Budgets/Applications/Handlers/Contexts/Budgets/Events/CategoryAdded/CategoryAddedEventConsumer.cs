using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.CategoryAdded;

/// <summary>
/// Consumes the <see cref="CategoryAddedEvent"/>.
/// </summary>
public sealed class CategoryAddedEventConsumer(
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<CategoryAddedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(CategoryAddedEvent @event, CancellationToken cancellationToken)
    {
        var request = new CategoryAdditionSynchronizationModel(
            @event.CategoryId,
            @event.BudgetId,
            @event.Category,
            @event.Limit,
            @event.CreatedAt);

        await budgetsSynchronizationService.SynchronizeCategoryAdditionAsync(request, cancellationToken);
    }
}