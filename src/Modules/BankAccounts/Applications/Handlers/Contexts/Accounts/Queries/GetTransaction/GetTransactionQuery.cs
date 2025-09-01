using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetTransaction;

/// <summary>
/// Represents a query to retrieve a transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record GetTransactionQuery(
    Guid AccountId,
    Guid TransactionId) : IQuery<TransactionResponse>;