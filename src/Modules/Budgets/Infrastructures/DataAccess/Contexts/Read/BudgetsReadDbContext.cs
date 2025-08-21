using Microsoft.EntityFrameworkCore;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read;

/// <summary>
/// Represents the read-side database context for the Budgets module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class BudgetsReadDbContext(
    DbContextOptions<BudgetsReadDbContext> options) : DbContext(options)
{
    /// <summary>Budgets.</summary>
    public DbSet<BudgetReadModel> Budgets { get; set; }

    /// <summary>BudgetItems.</summary>
    public DbSet<BudgetCategoryReadModel> BudgetCategories { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}