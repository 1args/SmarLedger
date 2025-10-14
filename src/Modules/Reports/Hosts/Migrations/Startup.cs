using Microsoft.Extensions.Logging;
using SmartLedger.Common.Hosts.Migrations.Abstractions;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Modules.Reports.Hosts.Migrations;

/// <summary>
/// Initiates database migrations for Transactions module.
/// </summary>
internal sealed class Startup(
    IDatabaseMigrationsService databaseMigrationsService,
    ILoggerFactory loggerFactory)
{
    /// <summary>
    /// Executes the database migration using the provided <see cref="ReportsDbContext"/>.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task StartMigrationAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Startup>();

        // Migrate ReportsReadDbContext first
        await databaseMigrationsService.ExecuteMigrationAsync<ReportsDbContext>(
            logger, cancellationToken);
    }
}