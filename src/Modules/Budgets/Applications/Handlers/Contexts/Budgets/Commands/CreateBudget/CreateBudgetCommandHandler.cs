using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.BudgetCreated;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudget;

/// <summary>
/// Handles the logic for processing <see cref="CreateBudgetCommand"/>.
/// </summary>
public sealed class CreateBudgetCommandHandler(
    IBudgetsService budgetService,
    IEventBus eventBus,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateBudgetCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> HandleAsync(CreateBudgetCommand command, CancellationToken cancellationToken)
    {
        var request = new BudgetCreationModel(
            command.UserId,
            command.Name,
            command.StartDate,
            command.EndDate,
            dateTimeProvider.UtcNow);

        var budgetId = await budgetService.CreateAsync(request, cancellationToken);

        await eventBus.PublishAsync(
            new BudgetCreatedEvent(
                budgetId,
                command.UserId,
                command.Name,
                command.StartDate,
                command.EndDate,
                dateTimeProvider.UtcNow),
            cancellationToken);

        return budgetId;
    }
}