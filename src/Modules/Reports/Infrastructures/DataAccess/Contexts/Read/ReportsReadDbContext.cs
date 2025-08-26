using Microsoft.EntityFrameworkCore;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;

/// <summary>
/// Represents the read-side database context for the Reports module.
/// </summary>
/// <param name="options">DbContext options.</param>
public sealed class ReportsReadDbContext(
    DbContextOptions<ReportsReadDbContext> options) : DbContext(options)
{
    /// <summary>Reports.</summary>
    public DbSet<ReportReadModel> Reports { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}