using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Common.Infrastructure.Extensions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

namespace SmartLedger.Modules.Transactions.Hosts.Extensions;

/// <summary>
/// Extensions for registering the Transactions module.
/// </summary>
public static class TransactionsModuleExtensions
{
    /// <summary>
    /// Registers the Transactions module.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddTransactionsModule(this IServiceCollection services)
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
            .AddDataAccess<TransactionsWriteDbContext, TransactionsWriteDbContextConfigurator>()
            .AddDataAccess<TransactionsReadDbContext, TransactionsReadDbContextConfigurator>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services.AddDateTimeProvider();

        services
            .AddScoped<IAccountsService, AccountsService>()
            .AddScoped<IAccountsSynchronizationService, AccountsSynchronizationService>()
            .AddScoped<ITransactionsService, ITransactionsService>()
            .AddScoped<ITransactionsSynchronizationService, TransactionsSynchronizationService>();

        services
            .AddHandlersFromAssembly(Assembly.GetAssembly(typeof(TransactionsService)));

        return services;
    }
}