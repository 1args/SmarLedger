using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events;

/// <summary>
/// Consumes the <see cref="BudgetCreatedEvent"/>.
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