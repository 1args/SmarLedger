using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudget;

/// <summary>
/// Represents a command to delete an account by ID.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
public sealed record DeleteBudgetCommand(
    Guid BudgetId) : ICommand;