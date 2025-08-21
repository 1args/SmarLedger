namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

/// <summary>
/// Represents a read model for a budget item.
/// </summary>
public sealed class BudgetCategoryReadModel
{
    /// <summary>Budget item ID.</summary>
    public Guid Id { get; set; }

    /// <summary>ID of the budget this item belongs to.</summary>
    public Guid BudgetId { get; set; }

    /// <summary>Name of the budget this item belongs to.</summary>
    public string BudgetName { get; set; } = string.Empty;

    /// <summary>ID of the user who owns this budget item.</summary>
    public Guid UserId { get; set; }

    /// <summary>Transaction category.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Spending limit for this category.</summary>
    public decimal Limit { get; set; }

    /// <summary>Amount already spent in this category.</summary>
    public decimal SpentAmount { get; set; }

    /// <summary>Current status of the budget item.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Start date of the budget period.</summary>
    public DateTime StartDate { get; set; }

    /// <summary>End date of the budget period.</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Date and time when budget item was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Date and time when budget item was last updated.</summary>
    public DateTime LastUpdatedAt { get; set; }
}