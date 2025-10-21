using Microsoft.Extensions.Logging;
using SmartLedger.Common.Host.Migrations.Abstractions;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.BankAccounts.Host.Migrations;

/// <summary>
/// Initiates database migrations for Transactions module.
/// </summary>
internal sealed class Startup(
    IDatabaseMigrationsService databaseMigrationsService,
    ILoggerFactory loggerFactory)
{
    /// <summary>
    /// Executes the database migration using the provided <see cref="BackAccountsWriteDbContext"/> and <see cref="BackAccountsReadDbContext"/>.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public async Task StartMigrationAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Startup>();

        // Migrate WriteDbContext first
        await databaseMigrationsService.ExecuteMigrationAsync<BackAccountsWriteDbContext>(
            logger, cancellationToken);

        // Then migrate ReadDbContext
        await databaseMigrationsService.ExecuteMigrationAsync<BackAccountsReadDbContext>(
            logger, cancellationToken);
    }
}