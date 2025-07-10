using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Model used to add a new category (budget item) to a budget.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Limit">Spending limit for the category.</param>
/// <param name="CreatedAt">Date and time when category was created.</param>
public sealed record CategoryAdditionModel(
    Guid BudgetId,
    TransactionCategory Category,
    decimal Limit,
    DateTime CreatedAt);