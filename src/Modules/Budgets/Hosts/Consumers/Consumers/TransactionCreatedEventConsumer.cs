using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.BankAccounts.Contracts.Events;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Host.Consumers.Consumers;

/// <summary>
/// Consumes the <see cref="TransactionCreatedEvent"/>.
/// </summary>
public sealed class TransactionCreatedEventConsumer(
    IBudgetsService budgetsService,
    IBudgetsSynchronizationService budgetsSynchronizationService) : IEventConsumer<TransactionCreatedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionCreatedEvent @event, CancellationToken cancellationToken)
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