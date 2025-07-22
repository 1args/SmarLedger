using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.Budgets;

/// <summary>
/// Filters budgets by user ID.
/// </summary>
/// <param name="userId">User ID.</param>
public sealed class BudgetByUserIdSpecification(Guid userId)
    : Specification<BudgetReadModel>(b => b.UserId == userId);