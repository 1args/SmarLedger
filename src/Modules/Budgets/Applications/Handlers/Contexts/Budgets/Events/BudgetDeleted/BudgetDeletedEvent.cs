using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.BudgetDeleted;

/// <summary>
/// Event triggered when a budget is deleted.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
public sealed record BudgetDeletedEvent(
    Guid BudgetId) : Event;