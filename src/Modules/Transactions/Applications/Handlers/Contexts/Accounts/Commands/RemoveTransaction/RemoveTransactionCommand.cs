using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;

/// <summary>
/// Represents a command to remove a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Amount">Amount.</param>
/// <param name="Type">Type of the transaction (Income or Expense).</param>
public sealed record RemoveTransactionCommand(
    Guid AccountId,
    decimal Amount,
    TransactionType Type) : ICommand;