using Microsoft.EntityFrameworkCore;

namespace SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read;

/// <summary>
/// Represents the read-side database context for the Budgets module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class BudgetsReadDbContext(
    DbContextOptions<BudgetsReadDbContext> options) : DbContext(options)
{


    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}