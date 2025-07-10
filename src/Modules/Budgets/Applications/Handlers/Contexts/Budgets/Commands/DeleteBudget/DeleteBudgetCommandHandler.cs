using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudget;

/// <summary>
/// Handles the logic for processing <see cref="DeleteBudgetCommand"/>.
/// </summary>
public sealed class DeleteBudgetCommandHandler(
    IBudgetsService budgetsService) : ICommandHandler<DeleteBudgetCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(DeleteBudgetCommand command, CancellationToken cancellationToken)
    {
        var request = new IdOnlyModel(command.BudgetId);

        await budgetsService.DeleteAsync(request, cancellationToken);
    }
}