using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.AddCategory;

/// <summary>
/// Handles the logic for processing <see cref="AddCategoryCommand"/>.
/// </summary>
public sealed class AddCategoryCommandHandler(
    IBudgetsService budgetsService,
    IEventBus eventBus,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<AddCategoryCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> HandleAsync(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = new CategoryAdditionModel(
            command.BudgetId,
            command.Category,
            command.Limit,
            dateTimeProvider.UtcNow);

        var categoryId = await budgetsService.AddCategoryAsync(request, cancellationToken);

        await eventBus.PublishAsync(new CategoryAddedEvent(
            categoryId,
            command.BudgetId,
            command.Category,
            command.Limit,
            dateTimeProvider.UtcNow), cancellationToken);

        return categoryId;
    }
}