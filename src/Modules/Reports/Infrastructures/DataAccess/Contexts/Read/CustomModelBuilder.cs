using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Infrastructures.DataAccess.Configurations;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Configurations;

namespace SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;

/// <summary>
/// Contains custom configuration logic for the EF Core read model.
/// </summary>
public static class CustomModelBuilder
{
    /// <summary>
    /// Applies custom configurations and conventions to the EF Core read model.
    /// </summary>
    /// <param name="modelBuilder">Model builder used to configure entity mappings.</param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("read")
            .SetDefaultDateTimeKind(DateTimeKind.Utc);

        modelBuilder
            .ApplyConfiguration(new ReportReadModelConfiguration());
    }
}