using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.DeleteTransaction;

/// <summary>
/// Represents a command to remove a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record DeleteTransactionCommand(
    Guid AccountId,
    Guid TransactionId) : ICommand;