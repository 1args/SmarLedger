using SmartLedger.Common.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Represents a model for synchronizing the addition of a category (budget item) to a budget.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="Category">Financial category.</param>
/// <param name="Limit">Spending limit for the category.</param>
/// <param name="CreatedAt">Date and time when category was created.</param>
public sealed record CategoryAdditionSynchronizationModel(
    Guid CategoryId,
    Guid BudgetId,
    FinancialCategory Category,
    decimal Limit,
    DateTime CreatedAt);