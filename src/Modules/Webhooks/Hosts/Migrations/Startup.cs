using Microsoft.Extensions.Logging;
using SmartLedger.Common.Hosts.Migrations.Abstractions;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;

namespace SmartLedger.Modules.Webhooks.Hosts.Migrations;

/// <summary>
/// Initiates database migrations for Transactions module.
/// </summary>
internal sealed class Startup(
    IDatabaseMigrationsService databaseMigrationsService,
    ILoggerFactory loggerFactory)
{
    /// <summary>
    /// Executes the database migration using the provided <see cref="WebhooksDbContext"/>.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task StartMigrationAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Startup>();

        // Migrate ReportsReadDbContext first
        await databaseMigrationsService.ExecuteMigrationAsync<WebhooksDbContext>(
            logger, cancellationToken);
    }
}