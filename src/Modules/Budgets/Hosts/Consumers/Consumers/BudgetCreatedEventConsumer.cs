using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Host.Consumers.Consumers;

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