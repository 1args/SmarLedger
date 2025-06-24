using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.CategorizeTransaction;

/// <summary>
/// Represents a command to categorize an existing transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewCategoryId">New category ID.</param>
public sealed record CategorizeTransactionCommand(
    Guid TransactionId,
    int NewCategoryId) : ICommand; 