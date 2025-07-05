using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Specifications;

/// <summary>
/// Filters accounts by a specific user ID.
/// </summary>
/// <param name="userId">User ID.</param>
public sealed class AccountByUserIdSpecification(Guid userId)
    : Specification<AccountReadModel>(a => a.UserId == userId);