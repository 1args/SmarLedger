using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.AddCategory;

public sealed class AddCategoryCommandHandler(
    IBudgetsService budgetsService,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<AddCategoryCommand, Guid>
{
    public async Task<Guid> HandleAsync(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = new CategoryAdditionModel(
            command.BudgetId,
            command.Category,
            command.Limit,
            dateTimeProvider.UtcNow);

        var categoryId = await budgetsService.AddCategoryAsync(request, cancellationToken);

        return categoryId;
    }
}