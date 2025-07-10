using SmartLedger.Common.Infrastructure.Extensions;
using SmartLedger.Modules.Budgets.Infrastructure.Configurators;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Write;

namespace SmartLedger.Hosts.Api.Extensions.Modules;

/// <summary>
/// Extensions for registering the Budgets module.
/// </summary>
public static class BudgetsModuleExtensions
{
    /// <summary>
    /// Registers the Budgets module.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddBudgetsModule(this IServiceCollection services)
    {
        services
            .AddInfrastructure()
            .AddApplications();

        return services;
    }

    /// <summary>
    /// Registers infrastructure components.
    /// </summary>
    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddDataAccess<BudgetsWriteDbContext, BudgetsWriteDbContextConfigurator>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {

        return services;
    }
}