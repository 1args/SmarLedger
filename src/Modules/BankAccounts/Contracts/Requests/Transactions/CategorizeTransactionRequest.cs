using SmartLedger.Common.Domain.Enums;

namespace SmartLedger.Modules.BankAccounts.Contracts.Requests.Transactions;

/// <summary>
/// Represents the request payload to categorize a transaction.
/// </summary>
/// <param name="NewCategory">New category.</param>
public sealed record CategorizeTransactionRequest(
    FinancialCategory NewCategory);