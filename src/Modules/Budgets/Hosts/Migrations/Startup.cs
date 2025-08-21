using Microsoft.Extensions.Logging;
using SmartLedger.Common.Hosts.Migrations.Abstractions;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.Budgets.Hosts.Migrations;

/// <summary>
/// Initiates database migrations for Transactions module.
/// </summary>
internal sealed class Startup(
    IDatabaseMigrationsService databaseMigrationsService,
    ILoggerFactory loggerFactory)
{
    /// <summary>
    /// Executes the database migration using the provided <see cref="BudgetsWriteDbContext"/> and <see cref="BudgetsReadDbContext"/>.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task StartMigrationAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Startup>();

        // Migrate WriteDbContext first
        await databaseMigrationsService.ExecuteMigrationAsync<BudgetsWriteDbContext>(
            logger, cancellationToken);

        // Then migrate ReadDbContext
        await databaseMigrationsService.ExecuteMigrationAsync<BudgetsReadDbContext>(
            logger, cancellationToken);
    }
}