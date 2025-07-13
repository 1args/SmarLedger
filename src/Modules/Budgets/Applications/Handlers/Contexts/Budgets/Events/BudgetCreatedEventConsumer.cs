using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events;

/// <summary>
/// Consumes the <see cref="BudgetCreatedEvent"/>.
/// </summary>
public sealed class BudgetCreatedEventConsumer(
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<BudgetCreatedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(BudgetCreatedEvent @event, CancellationToken cancellationToken)
    {
        var request = new BudgetCreationSynchronizationModel(
            @event.BudgetId,
            @event.UserId,
            @event.Name,
            @event.StartDate,
            @event.EndDate,
            @event.CreatedAt);

        await budgetsSynchronizationService.SynchronizeBudgetCreationAsync(request, cancellationToken);
    }
} 