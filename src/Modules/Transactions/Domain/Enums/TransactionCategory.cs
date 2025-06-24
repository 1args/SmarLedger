namespace SmartLedger.Modules.Transactions.Domain.Enums;

/// <summary>
/// Enumeration of predefined transaction categories.
/// </summary>
public enum TransactionCategory
{
    /// <summary>Unknown category.</summary>
    Unknown = 1,

    /// <summary>Food and groceries.</summary>
    Food = 2,

    /// <summary>Transportation and commuting.</summary>
    Transport = 3,

    /// <summary>Salary or income.</summary>
    Salary = 4,

    /// <summary>Utility payments (e.g., electricity, water).</summary>
    Utilities = 5,

    /// <summary>Leisure and entertainment expenses.</summary>
    Entertainment = 6,

    /// <summary>Other or miscellaneous expenses.</summary>
    Other = 7
}