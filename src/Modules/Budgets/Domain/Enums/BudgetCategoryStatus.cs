namespace SmartLedger.Modules.Budgets.Domain.Enums;

/// <summary>
/// Indicates the current status of a budget category.
/// </summary>
public enum BudgetCategoryStatus
{
    /// <summary>Item is within the set limit.</summary>
    Active = 0,

    /// <summary>Item has exceeded the limit.</summary>
    Exceeded = 1,

    /// <summary>Activity time has expired</summary>
    Inactive = 2
}