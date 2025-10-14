using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Hosts.Api.Extensions.Modules;

/// <summary>
/// Extensions for registering the Transactions module.
/// </summary>
public static class BankAccountsModuleExtensions
{
    /// <summary>
    /// Registers the Transactions module.
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
            .AddDataAccess<BackAccountsWriteDbContext, BankAccountsWriteDbContextConfigurator>()
            .AddDataAccess<BackAccountsReadDbContext, BankAccountsReadDbContextConfigurator>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IAccountsService, AccountsService>()
            .AddScoped<IAccountsSynchronizationService, AccountsSynchronizationService>()
            .AddScoped<IAccountsRetrievalService, AccountsRetrievalService>();

        services
            .AddHandlersFromAssembly(typeof(CreateAccountCommandHandler).Assembly);

        return services;
    }
}