namespace SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;

/// <summary>
/// Represents a response containing the details of a budget category in list.
/// </summary>
/// <param name="Category">Transaction category.</param>
/// <param name="Limit">Spending limit for this category.</param>
/// <param name="SpentAmount">Amount already spent in this category.</param>
/// <param name="Status">Current status of the budget item.</param>
/// <param name="CreatedAt">Date and time when budget item was created.</param>
/// <param name="LastUpdatedAt">Date and time when budget item was last updated.</param>
public sealed record BudgetCategoryListItem(
    Guid BudgetCategoryId,
    string Category,
    decimal Limit,
    decimal SpentAmount,
    string Status,
    DateTime CreatedAt,
    DateTime LastUpdatedAt);