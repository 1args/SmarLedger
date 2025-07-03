using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;

/// <summary>
/// Represents a command to add a transaction to an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
public sealed record AddTransactionCommand(
    Guid AccountId,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    string Notes) : ICommand<Guid>;