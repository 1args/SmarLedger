using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.DeleteAccount;

/// <summary>
/// Represents a command to delete an account by ID.
/// </summary>
/// <param name="AccountId">Account ID.</param>
public sealed record DeleteAccountCommand(
    Guid AccountId) : ICommand;