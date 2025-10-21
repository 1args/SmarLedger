using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Contracts.Events;

/// <summary>
/// Event triggered when a budget is deleted.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
public sealed record BudgetDeletedEvent(
    Guid BudgetId) : Event;