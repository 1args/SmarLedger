namespace SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;

/// <summary>
/// Represents a response containing the details of an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="UserId">ID of the user who owns this account.</param>
/// <param name="Name">Account name.</param>
/// <param name="Balance"> Current balance.</param>
/// <param name="CreatedAt">Date and time when account was created.</param>
/// <param name="LastUpdatedAt">Date and time when account was last updated.</param>
public record AccountResponse(
    Guid AccountId,
    Guid UserId,
    string Name,
    decimal Balance,
    DateTime CreatedAt,
    DateTime LastUpdatedAt);