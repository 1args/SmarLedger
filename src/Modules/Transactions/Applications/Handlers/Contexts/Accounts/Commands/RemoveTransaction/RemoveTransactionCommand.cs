using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;

/// <summary>
/// Represents a command to remove a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record RemoveTransactionCommand(
    Guid AccountId,
    Guid TransactionId) : ICommand;