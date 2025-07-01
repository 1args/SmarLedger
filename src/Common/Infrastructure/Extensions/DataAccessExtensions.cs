using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Common.Infrastructure.Configurations;
using SmartLedger.Common.Infrastructure.Repositories;
using SmartLedger.Common.Infrastructure.Transactions;

namespace SmartLedger.Common.Infrastructure.Extensions;

/// <summary>
/// Extension for configuring data access.
/// </summary>
public static class DataAccessExtensions
{
    /// <summary>
    /// Adds EF Core with Npgsql support.
    /// </summary>
    /// <typeparam name="TDbContext">Type of DbContext.</typeparam>
    /// <typeparam name="TDbContextConfigurator">DbContext configurator implementing <see cref="IDbContextOptionsConfigurator{TDbContext}"/>.</typeparam>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddDataAccess<TDbContext, TDbContextConfigurator>(this IServiceCollection services)
        where TDbContext : DbContext
        where TDbContextConfigurator : class, IDbContextOptionsConfigurator<TDbContext>
    {
        services.AddEntityFrameworkNpgsql()
            .AddDbContextPool<TDbContext>(Configure<TDbContext>);

        services
            .AddSingleton<IDbContextOptionsConfigurator<TDbContext>, TDbContextConfigurator>()
            .AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>())
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped<ITransactionManager, TransactionManager>();

        return services;
    }

    internal static void Configure<TDbContext>(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder)
        where TDbContext : DbContext
    {
        var configurator = serviceProvider.GetRequiredService<IDbContextOptionsConfigurator<TDbContext>>();
        configurator.Configure((DbContextOptionsBuilder<TDbContext>)optionsBuilder);
    }
}