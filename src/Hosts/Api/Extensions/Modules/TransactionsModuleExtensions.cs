using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Hosts.Api.Extensions.Modules;

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
            .AddDataAccess<TransactionsWriteDbContext, TransactionsWriteDbContextConfigurator>()
            .AddDataAccess<TransactionsReadDbContext, TransactionsReadDbContextConfigurator>();

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
            .AddScoped<ITransactionsService, TransactionsService>()
            .AddScoped<ITransactionsSynchronizationService, TransactionsSynchronizationService>()
            .AddScoped<ITransactionsRetrievalService, TransactionsRetrievalService>();

        services
            .AddHandlersFromAssembly(typeof(CreateAccountCommandHandler).Assembly);

        return services;
    }
}