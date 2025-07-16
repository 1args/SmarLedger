using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.RemoveCategory;

/// <summary>
/// Represents a command to remove a category by ID.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="CategoryId">Category ID.</param>
public sealed record RemoveCategoryCommand(
    Guid BudgetId,
    Guid CategoryId) : ICommand;