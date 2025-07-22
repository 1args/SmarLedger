using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;

/// <summary>
/// Filters budget categories by a specific status.
/// </summary>
/// <param name="status">Optional status.</param>
public sealed class BudgetCategoryByStatusSpecification(string? status)
    : Specification<BudgetCategoryReadModel>(bc => bc.Status == status);