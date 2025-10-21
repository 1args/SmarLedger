using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Host.Migrations.Abstractions;

namespace SmartLedger.Common.Host.Migrations;

/// <summary>
/// Implementation of <see cref="IDatabaseMigrationsService"/> that executes EF Core migrations.
/// </summary>
public sealed class DatabaseMigrationsService(
    IServiceProvider serviceProvider) : IDatabaseMigrationsService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider 
        ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <inheritdoc />
    public async Task ExecuteMigrationAsync<TDbContext>(ILogger logger, CancellationToken cancellationToken) 
        where TDbContext : DbContext
    {
        var dbContextName = typeof(TDbContext).Name;

        logger.LogInformation("Starting database migration for {DbContext}", dbContextName);

        var dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<TDbContext>()
            ?? throw new ArgumentNullException($"Failed to get DbContext with the following name: '{dbContextName}'.");

        try
        {
            logger.LogInformation("Attempting to apply migrations for {DbContext}", dbContextName);

            await dbContext.Database.MigrateAsync(cancellationToken);

            logger.LogInformation("Successfully applied migrations for {DbContext}", dbContextName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations for {DbContext}", dbContextName);
        }
    }
}