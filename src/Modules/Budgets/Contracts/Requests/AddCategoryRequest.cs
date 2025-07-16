using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Contracts.Requests;

/// <summary>
/// Represents a request to add a new category (budget item) to a budget.
/// </summary>
/// <param name="Category">Transaction category.</param>
/// <param name="Limit">Spending limit for the category.</param>
public sealed record AddCategoryRequest(
    TransactionCategory Category,
    decimal Limit);