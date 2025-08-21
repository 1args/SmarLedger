namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

/// <summary>
/// Represents a read model for a budget.
/// </summary>
public sealed class BudgetReadModel
{
    /// <summary>Budget ID.</summary>
    public Guid Id { get; set; }

    /// <summary>ID of the user who owns this budget.</summary>
    public Guid UserId { get; set; }

    /// <summary>Budget name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Start date of the budget period.</summary>
    public DateTime StartDate { get; set; }

    /// <summary>End date of the budget period.</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Date and time when budget was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Date and time when budget was last updated.</summary>
    public DateTime LastUpdatedAt { get; set; }
}