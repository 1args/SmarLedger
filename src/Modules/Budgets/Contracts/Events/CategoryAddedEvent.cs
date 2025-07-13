using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Contracts.Events;

/// <summary>
/// Event triggered when a new category is added.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Limit">Spending limit for the category.</param>
/// <param name="CreatedAt">Date and time when category was created.</param>
public sealed record CategoryAddedEvent(
    Guid CategoryId,
    Guid BudgetId,
    TransactionCategory Category,
    decimal Limit,
    DateTime CreatedAt) : Event;