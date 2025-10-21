using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Modules.BankAccounts.Host.Consumers.Extensions;

/// <summary>
/// Extensions for registering the BankAccounts module components for consumers.
/// </summary>
public static class BankAccountsModuleExtensions
{
    /// <summary>
    /// Registers the BankAccounts module components.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddBankAccountsModule(this IServiceCollection services)
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
            .AddDataAccess<BackAccountsReadDbContext, BankAccountsReadDbContextConfigurator>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IAccountsSynchronizationService, AccountsSynchronizationService>();

        return services;
    }
}