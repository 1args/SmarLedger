using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;

/// <summary>
/// Represents a command to create a new account.
/// </summary>
/// <param name="Name">Name.</param>
/// <param name="UserId">ID of the user who owns the account.</param>
public sealed record CreateAccountCommand(
    string Name,
    Guid UserId) : ICommand;