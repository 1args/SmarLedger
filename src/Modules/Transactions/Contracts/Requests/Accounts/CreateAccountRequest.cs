namespace SmartLedger.Modules.Transactions.Contracts.Requests.Accounts;

/// <summary>
/// Represents the request payload to create a new account.
/// </summary>
/// <param name="Name">Account name.</param>
/// <param name="UserId">User Id.</param>
public sealed record CreateAccountRequest(
    string Name,
    Guid UserId);