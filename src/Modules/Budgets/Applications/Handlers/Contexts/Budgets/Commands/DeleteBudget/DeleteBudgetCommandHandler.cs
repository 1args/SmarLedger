using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.BudgetDeleted;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudget;

/// <summary>
/// Handles the logic for processing <see cref="DeleteBudgetCommand"/>.
/// </summary>
public sealed class DeleteBudgetCommandHandler(
    IBudgetsService budgetsService,
    IEventBus eventBus) : ICommandHandler<DeleteBudgetCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(DeleteBudgetCommand command, CancellationToken cancellationToken)
    {
        var request = new IdOnlyModel(command.BudgetId);

        await budgetsService.DeleteAsync(request, cancellationToken);

        await eventBus.PublishAsync(new BudgetDeletedEvent(command.BudgetId), cancellationToken);
    }
}