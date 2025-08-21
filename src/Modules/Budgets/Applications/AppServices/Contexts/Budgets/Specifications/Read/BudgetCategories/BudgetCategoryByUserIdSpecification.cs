using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;

/// <summary>
/// Filters budget items by a specific user ID.
/// </summary>
/// <param name="userId">User ID.</param>
public sealed class BudgetCategoryByUserIdSpecification(Guid userId)
    : Specification<BudgetCategoryReadModel>(bc => bc.UserId == userId);