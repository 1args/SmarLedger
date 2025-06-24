using Microsoft.EntityFrameworkCore;

namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

/// <summary>
/// A static class responsible for accepting model configurations.
/// </summary>
public static class CustomModelBuilder
{
    /// <summary>
    /// Configuring models during their creation.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }
}