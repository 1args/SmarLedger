using Microsoft.Extensions.Logging;
using SmartLedger.Common.Hosts.Migrations.Abstractions;
using SmartLedger.Common.Infrastructure.Events;

namespace SmartLedger.Hosts.Migrations;

internal sealed class Startup(
    IDatabaseMigrationsService databaseMigrationsService,
    ILoggerFactory loggerFactory)
{
    public async Task StartMigrationAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Startup>();

        await databaseMigrationsService.ExecuteMigrationAsync<OutboxDbContext>(
            logger, cancellationToken);
    }
}
