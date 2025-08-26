namespace SmartLedger.Modules.Transactions.Contracts.Requests.Accounts;

/// <summary>
/// Represents the request payload to create a new account.
/// </summary>
/// <param name="Name">Account name.</param>
public sealed record CreateAccountRequest(
    string Name);