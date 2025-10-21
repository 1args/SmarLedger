using SmartLedger.Common.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;

/// <summary>
/// Model used to create a new category (budget item) to a budget.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="Category">Financial category.</param>
/// <param name="Limit">Spending limit for the category.</param>
/// <param name="CreatedAt">Date and time when category was created.</param>
public sealed record BudgetCategoryCreationModel(
    Guid BudgetId,
    FinancialCategory Category,
    decimal Limit,
    DateTime CreatedAt);