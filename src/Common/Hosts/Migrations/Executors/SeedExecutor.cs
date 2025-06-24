using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Hosts.Migrations.Abstractions;

namespace SmartLedger.Common.Hosts.Migrations.Executors;

/// <inheritdoc />
public class SeedExecutor<TDbContext>(TDbContext dbContext) : ISeedExecutor 
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc />
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var type = typeof(ISeed<TDbContext>);

        var seedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => type.IsAssignableFrom(t) && !t.IsInterface);

        foreach (var seedType in seedTypes)
        {
            var seed = (ISeed<TDbContext>?)Activator.CreateInstance(seedType);

            if (seedType is not null)
            {
                await seed!.SeedAsync(_dbContext, cancellationToken);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}