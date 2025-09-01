namespace SmartLedger.Modules.BankAccounts.Contracts.Responses.Accounts;

/// <summary>
/// Represents a response containing the details of an account in list.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Name">Account name.</param>
/// <param name="Balance"> Current balance.</param>
/// <param name="CreatedAt">Date and time when account was created.</param>
/// <param name="LastUpdatedAt">Date and time when account was last updated.</param>
public record AccountListItem(
    Guid AccountId,
    string Name,
    decimal Balance,
    DateTime CreatedAt,
    DateTime LastUpdatedAt);