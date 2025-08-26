using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetAccount;

/// <summary>
/// Represents a query to retrieve an account.
/// </summary>
/// <param name="UserId">User ID.</param>
public sealed record GetAccountQuery(
    Guid UserId) : IQuery<AccountResponse>;