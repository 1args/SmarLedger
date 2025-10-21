using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SmartLedger.Common.Host.Migrations.Abstractions;

/// <summary>
/// Interface for executing EF Core database migrations for a specified <see cref="DbContext"/>.
/// </summary>
public interface IDatabaseMigrationsService
{
    /// <summary>
    /// Executes any pending EF Core migrations for the specified <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext type.</typeparam>
    /// <param name="logger">Logger used for reporting progress and errors.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ExecuteMigrationAsync<TDbContext>(ILogger logger, CancellationToken cancellationToken)
        where TDbContext : DbContext;
}