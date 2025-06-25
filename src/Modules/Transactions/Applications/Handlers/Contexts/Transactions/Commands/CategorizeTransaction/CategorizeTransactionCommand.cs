using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.CategorizeTransaction;

/// <summary>
/// Represents a command to categorize an existing transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewCategory">New category.</param>
public sealed record CategorizeTransactionCommand(
    Guid TransactionId,
    TransactionCategory NewCategory) : ICommand; 