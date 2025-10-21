using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Events;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudgetCategory;

/// <summary>
/// Handles the logic for processing <see cref="CreateBudgetCategoryCommand"/>.
/// </summary>
public sealed class CreateBudgetCategoryCommandHandler(
    IBudgetsService budgetsService,
    IEventBus bus,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateBudgetCategoryCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> HandleAsync(CreateBudgetCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = new BudgetCategoryCreationModel(
            command.BudgetId,
            command.Category,
            command.Limit,
            dateTimeProvider.UtcNow);

        var categoryId = await budgetsService.CreateBudgetCategoryAsync(request, cancellationToken);

        await bus.PublishAsync(
            new BudgetCategoryCreatedEvent(
                categoryId,
                command.BudgetId,
                command.Category,
                command.Limit,
                dateTimeProvider.UtcNow), 
            cancellationToken);

        return categoryId;
    }
}