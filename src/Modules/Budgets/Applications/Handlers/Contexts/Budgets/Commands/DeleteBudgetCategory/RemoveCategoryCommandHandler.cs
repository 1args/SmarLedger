using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudgetCategory;

/// <summary>
/// Handles the logic for processing <see cref="RemoveCategoryCommand"/>.
/// </summary>
public sealed class RemoveCategoryCommandHandler(
    IBudgetsService budgetsService,
    IEventBus bus) : ICommandHandler<RemoveCategoryCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RemoveCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = new BudgetCategoryDeletionModel(command.BudgetId, command.CategoryId);

        await budgetsService.DeleteBudgetCategoryAsync(request, cancellationToken);

        await bus.PublishAsync(
            new BudgetCategoryDeletedEvent(command.CategoryId),
            cancellationToken);
    }
}