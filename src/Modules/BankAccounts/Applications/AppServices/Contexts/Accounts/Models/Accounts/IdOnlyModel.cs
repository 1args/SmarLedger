namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;

/// <summary>
/// Model containing only the account ID.
/// </summary>
/// <param name="AccountId">Account ID.</param>
public sealed record IdOnlyModel(
    Guid AccountId);