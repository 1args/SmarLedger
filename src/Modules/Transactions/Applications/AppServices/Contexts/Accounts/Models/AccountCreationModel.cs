namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Model used to create a new account.
/// </summary>
/// <param name="Name">Name of the account.</param>
/// <param name="CreatedAt">Date and time the account was created.</param>
public sealed record AccountCreationModel(
    string Name,
    DateTime CreatedAt);