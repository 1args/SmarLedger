namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Model used to create a new account in synchronization context.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Name">Name of the account.</param>
/// <param name="UserId">ID of the user who owns the account.</param>
/// <param name="CreatedAt">Date and time the account was created.</param>
public sealed record AccountCreationSynchronizationModel(
    Guid AccountId,
    string Name,
    Guid UserId,
    DateTime CreatedAt);