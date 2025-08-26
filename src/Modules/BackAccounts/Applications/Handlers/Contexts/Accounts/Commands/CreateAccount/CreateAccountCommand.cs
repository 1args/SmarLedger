using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;

/// <summary>
/// Represents a command to create a new account.
/// </summary>
/// <param name="Name">Name.</param>
public sealed record CreateAccountCommand(
    string Name) : ICommand<Guid>;