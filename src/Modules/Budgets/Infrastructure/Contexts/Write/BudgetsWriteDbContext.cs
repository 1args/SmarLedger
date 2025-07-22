using Microsoft.EntityFrameworkCore;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Domain.Entities;

namespace SmartLedger.Modules.Budgets.Infrastructure.Contexts.Write;

/// <summary>
/// Represents the write-side database context for the Budgets module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class BudgetsWriteDbContext(
    DbContextOptions<BudgetsWriteDbContext> options) : DbContext(options)
{
    /// <summary>Budgets.</summary>
    public DbSet<Budget> Budgets { get; set; }

    /// <summary>BudgetItems.</summary>
    public DbSet<BudgetCategory> BudgetCategories { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}