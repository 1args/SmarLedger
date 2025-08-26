using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Specifications.Accounts;

/// <summary>
/// Filters accounts by a specific user ID.
/// </summary>
/// <param name="userId">User ID.</param>
public sealed class AccountByUserIdSpecification(Guid userId)
    : Specification<AccountReadModel>(a => a.UserId == userId);