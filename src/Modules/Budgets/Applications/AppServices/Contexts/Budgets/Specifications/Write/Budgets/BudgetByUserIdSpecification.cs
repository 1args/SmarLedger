using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Domain.Aggregates;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Write.Budgets;

/// <summary>
/// Filters budgets by a specific user ID.
/// </summary>
/// <param name="userId">User ID.</param>
public sealed class BudgetByUserIdSpecification(Guid userId)
    : Specification<Budget>(b => b.UserId == userId);