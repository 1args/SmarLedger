using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;

/// <summary>
/// Represents a command to add a transaction to an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Amount">Amount.</param>
/// <param name="Type">Type of the transaction (Income or Expense).</param>
public sealed record AddTransactionCommand(
    Guid AccountId,
    decimal Amount,
    TransactionType Type) : ICommand;