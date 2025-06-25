namespace SmartLedger.Modules.Transactions.Domain.Enums;

/// <summary>
/// Enumeration of predefined transaction categories.
/// </summary>
public enum TransactionCategory
{
    /// <summary>Unknown category.</summary>
    Unknown = 0,

    /// <summary>Food and groceries.</summary>
    Food = 1,

    /// <summary>Transportation and commuting.</summary>
    Transport = 2,

    /// <summary>Salary or income.</summary>
    Salary = 3,

    /// <summary>Utility payments (e.g., electricity, water).</summary>
    Utilities = 4,

    /// <summary>Leisure and entertainment expenses.</summary>
    Entertainment = 5,

    /// <summary>Other or miscellaneous expenses.</summary>
    Other = 6
}