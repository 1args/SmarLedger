using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.RemoveCategory;

/// <summary>
/// Handles the logic for processing <see cref="DeleteBudgetCommand"/>.
/// </summary>
public sealed class RemoveCategoryCommandHandler(
    IBudgetsService budgetsService) : ICommandHandler<RemoveCategoryCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RemoveCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = new CategoryRemovalModel(command.CategoryId);

        await budgetsService.RemoveCategoryAsync(request, cancellationToken);
    }
}