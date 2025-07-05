using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

/// <summary>
/// Filters transactions by a specific account ID.
/// </summary>
/// <param name="accountId">Account ID.</param>
public sealed class TransactionByAccountIdSpecification(Guid accountId)
    : Specification<TransactionReadModel>(t => t.AccountId == accountId);