using Microsoft.Extensions.Logging;
using SmartLedger.Common.Hosts.Migrations.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Events;

namespace SmartLedger.Hosts.Outbox.Migrations;

/// <summary>
/// Initiates database migrations for Outbox.
/// </summary>
internal sealed class Startup(
    IDatabaseMigrationsService databaseMigrationsService,
    ILoggerFactory loggerFactory)
{
    /// <summary>
    /// Executes the database migration using the provided <see cref="OutboxDbContext"/>.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task StartMigrationAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Startup>();

        await databaseMigrationsService.ExecuteMigrationAsync<OutboxDbContext>(
            logger, cancellationToken);
    }
}