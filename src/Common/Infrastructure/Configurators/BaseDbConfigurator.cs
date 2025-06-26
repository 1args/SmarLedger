using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructure.Configurations;

namespace SmartLedger.Common.Infrastructure.Configurators;

/// <summary>
/// Base class of database configuration.
/// </summary>
/// <typeparam name="TDbContext">Type of DbContext.</typeparam>
public abstract class BaseDbContextConfigurator<TDbContext>(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : IDbContextOptionsConfigurator<TDbContext> where TDbContext : DbContext
{
    /// <inheritdoc />
    public void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionString}' could not be found.");
        }

        optionsBuilder.UseLoggerFactory(loggerFactory)
            .UseNpgsql(connectionString, builder => builder.CommandTimeout(60));
    }
}