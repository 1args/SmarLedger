using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.BankAccounts.Domain.Enums;
using SmartLedger.Modules.Budgets.Domain.Aggregates;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Write.BudgetCategories;

/// <summary>
/// Filters budgets by a specific category.
/// </summary>
/// <param name="category">Category.</param>
public sealed class BudgetByCategorySpecification(TransactionCategory category)
    : Specification<Budget>(b => b.Categories.Any(bi => bi.Category == category));