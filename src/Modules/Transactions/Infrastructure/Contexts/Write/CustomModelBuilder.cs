using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Infrastructure.Configurations;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write.Configurations;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

/// <summary>
/// Contains custom configuration logic for the EF Core write model.
/// </summary>
public static class CustomModelBuilder
{
    /// <summary>
    /// Applies custom configurations and conventions to the EF Core write model.
    /// </summary>
    /// <param name="modelBuilder">Model builder used to configure entity mappings.</param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("write")
            .SetDefaultDateTimeKind(DateTimeKind.Utc);

        modelBuilder
            .ApplyConfiguration(new AccountConfiguration())
            .ApplyConfiguration(new TransactionConfiguration());
    }
}