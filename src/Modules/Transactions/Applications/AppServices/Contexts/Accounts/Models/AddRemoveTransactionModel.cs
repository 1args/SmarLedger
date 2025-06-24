using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Model used to add or remove a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Amount">Amount of the transaction.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
public sealed record AddRemoveTransactionModel(
    Guid AccountId, 
    decimal Amount, 
    TransactionType Type);