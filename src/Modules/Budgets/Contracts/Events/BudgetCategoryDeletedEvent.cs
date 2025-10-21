using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Contracts.Events;

/// <summary>
/// Event triggered when a category is removed from a budget.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
public sealed record BudgetCategoryDeletedEvent(
    Guid CategoryId) : Event;