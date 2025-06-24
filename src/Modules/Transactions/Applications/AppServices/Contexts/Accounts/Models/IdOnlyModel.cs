namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Model containing only the account ID.
/// </summary>
/// <param name="AccountId">Account ID.</param>
public sealed record IdOnlyModel(
    Guid AccountId);