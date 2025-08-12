using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudget;

/// <summary>
/// Represents a command to create a new budget.
/// </summary>
/// <param name="Name">Name of the budget.</param>
/// <param name="StartDate">Start date of the budget period.</param>
/// <param name="EndDate">End date of the budget period.</param>
public sealed record CreateBudgetCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate) : ICommand<Guid>;