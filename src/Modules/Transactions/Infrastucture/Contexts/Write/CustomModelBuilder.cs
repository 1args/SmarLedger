using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Infrastructure.Configurations;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write.Configurations;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

/// <summary>
/// Contains custom configuration logic for the EF Core model.
/// </summary>
public static class CustomModelBuilder
{
    /// <summary>
    /// Applies custom configurations and conventions to the EF Core model.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure entity mappings.</param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetDefaultDateTimeKind(DateTimeKind.Utc);

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
    }
}