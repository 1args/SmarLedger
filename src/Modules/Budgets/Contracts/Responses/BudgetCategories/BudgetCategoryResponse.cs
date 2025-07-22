namespace SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;

/// <summary>
/// Represents a response containing details of a budget category.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="UserId">User ID.</param>
/// <param name="BudgetName">Name of the budget this item belongs to..</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Limit">Spending limit for this category.</param>
/// <param name="SpentAmount">Amount already spent in this category.</param>
/// <param name="Status">Current status of the budget item.</param>
/// <param name="StartDate">Start date of the budget period.</param>
/// <param name="EndDate">End date of the budget period.</param>
/// <param name="CreatedAt">Date and time when budget item was created.</param>
/// <param name="LastUpdatedAt">Date and time when budget item was last updated.</param>
public sealed record BudgetCategoryResponse(
    Guid BudgetId,
    Guid UserId,
    string BudgetName, 
    string Category,
    decimal Limit,
    decimal SpentAmount,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedAt,
    DateTime LastUpdatedAt);