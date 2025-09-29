using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.BankAccounts.Contracts.Events;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.TransactionAdded;

/// <summary>
/// Consumes the <see cref="TransactionAddedIntegrationEvent"/>.
/// </summary>
public sealed class TransactionAddedEventConsumer(
    IBudgetsService budgetsService,
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<TransactionAddedIntegrationEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionAddedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var request = new TransactionModificationModel(
            @event.UserId,
            @event.Amount,
            @event.Type,
            @event.Category,
            @event.CreatedAt);

        await Task.WhenAll(
            budgetsService.AddSpendingAmountAsync(request, cancellationToken),
            budgetsSynchronizationService.SynchronizeAdditionSpendingAmountAsync(request, cancellationToken));
    }
}