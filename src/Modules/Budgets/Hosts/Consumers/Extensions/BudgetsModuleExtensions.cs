using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.Budgets.Host.Consumers.Extensions;

/// <summary>
/// Extensions for registering the Budgets module components for consumers.
/// </summary>
public static class BudgetsModuleExtensions
{
    /// <summary>
    /// Registers the Budgets module components.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddBudgetsModule(this IServiceCollection services)
    {
        services
            .AddInfrastructures()
            .AddApplications();

        return services;
    }

    /// <summary>
    /// Registers infrastructure components.
    /// </summary>
    private static IServiceCollection AddInfrastructures(this IServiceCollection services)
    {
        services
            .AddDataAccess<BudgetsWriteDbContext, BudgetsWriteDbContextConfigurator>()
            .AddDataAccess<BudgetsReadDbContext, BudgetsReadDbContextConfigurator>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IBudgetsService, BudgetsService>()
            .AddScoped<IBudgetsSynchronizationService, BudgetsSynchronizationService>();

        return services;
    }
}