using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Specifications.Transactions;

/// <summary>
/// Filters transactions by a specific account ID.
/// </summary>
/// <param name="accountId">Account ID.</param>
public sealed class TransactionByAccountIdSpecification(Guid accountId)
    : Specification<TransactionReadModel>(t => t.AccountId == accountId);