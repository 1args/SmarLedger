using Microsoft.Extensions.Logging;
using SmartLedger.Common.Hosts.Migrations.Abstractions;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.Transactions.Hosts.Migrations;

/// <summary>
/// Initiates database migrations for Transactions module.
/// </summary>
internal sealed class Startup(
    IDatabaseMigrationsService databaseMigrationsService,
    ILoggerFactory loggerFactory)
{
    /// <summary>
    /// Executes the database migration using the provided <see cref="TransactionsWriteDbContext"/> and <see cref="TransactionsReadDbContext"/>.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task StartMigrationAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Startup>();

        // Migrate WriteDbContext first
        await databaseMigrationsService.ExecuteMigrationAsync<TransactionsWriteDbContext>(
            logger, cancellationToken);

        // Then migrate ReadDbContext
        await databaseMigrationsService.ExecuteMigrationAsync<TransactionsReadDbContext>(
            logger, cancellationToken);
    }
}