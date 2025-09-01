namespace SmartLedger.Common.Domain.Enums;

/// <summary>
/// Represents the type of financial transaction.
/// </summary>
public enum TransactionType
{
    /// <summary>Income transaction (funds coming into the account).</summary>
    Income = 1,

    /// <summary>Expense transaction (funds going out from the account).</summary>
    Expense = 2,
}