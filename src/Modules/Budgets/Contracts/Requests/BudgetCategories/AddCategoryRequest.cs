using SmartLedger.Common.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Contracts.Requests.BudgetCategories;

/// <summary>
/// Represents a request to add a new category (budget item) to a budget.
/// </summary>
/// <param name="Category">Financial category.</param>
/// <param name="Limit">Spending limit for the category.</param>
public sealed record AddCategoryRequest(
    FinancialCategory Category,
    decimal Limit);