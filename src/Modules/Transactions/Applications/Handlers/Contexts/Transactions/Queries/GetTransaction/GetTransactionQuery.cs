using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Queries.GetTransaction;

/// <summary>
/// Represents a query to retrieve a transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record GetTransactionQuery(
    Guid TransactionId) : IQuery<TransactionResponse>;