using Microsoft.EntityFrameworkCore;

namespace SmartLedger.Common.Hosts.Migrations.Abstractions;

/// <summary>
/// Defines a contract for seeding initial data into a specific <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TDbContext">DbContext type.</typeparam>
internal interface ISeed<in TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// Executes the seed.
    /// </summary>
    /// <param name="dbContext">DbContext.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SeedAsync(TDbContext dbContext, CancellationToken cancellationToken);
}