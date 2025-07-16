using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events;

/// <summary>
/// Consumes the <see cref="BudgetCreatedEvent"/>.
/// </summary>
public sealed class CategoryRemovedEventConsumer(
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<CategoryRemovedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(CategoryRemovedEvent @event, CancellationToken cancellationToken)
    {
        var request = new CategoryRemovalSynchronizationModel(@event.CategoryId);

        await budgetsSynchronizationService.SynchronizeCategoryRemovalAsync(request, cancellationToken);
    }
}