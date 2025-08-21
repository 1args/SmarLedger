using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.CategoryRemoved;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.RemoveCategory;

/// <summary>
/// Handles the logic for processing <see cref="RemoveCategoryCommand"/>.
/// </summary>
public sealed class RemoveCategoryCommandHandler(
    IBudgetsService budgetsService,
    IEventBus eventBus) : ICommandHandler<RemoveCategoryCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RemoveCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = new CategoryRemovalModel(command.BudgetId, command.CategoryId);

        await budgetsService.RemoveCategoryAsync(request, cancellationToken);

        await eventBus.PublishAsync(
            new CategoryRemovedEvent(command.CategoryId), cancellationToken);
    }
}