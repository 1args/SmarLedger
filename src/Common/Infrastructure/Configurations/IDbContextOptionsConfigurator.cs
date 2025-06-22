using Microsoft.EntityFrameworkCore;

namespace SmartLedger.Common.Infrastructure.Configurations;

/// <summary>
/// Provides an interface to configure <see cref="DbContextOptionsBuilder{TContext}"/> for a specific DbContext.
/// </summary>
/// <typeparam name="TDbContext">Type of the DbContext.</typeparam>
public interface IDbContextOptionsConfigurator<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// Configures the DbContext options.
    /// </summary>
    /// <param name="optionsBuilder">Builder used to configure the DbContext options.</param>
    void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder);
}