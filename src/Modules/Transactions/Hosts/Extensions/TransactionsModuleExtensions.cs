using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Common.Infrastructure.Extensions;
using SmartLedger.Modules.Transactions.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

namespace SmartLedger.Modules.Transactions.Hosts.Extensions;

public static class TransactionsModuleExtensions
{
    public static IServiceCollection AddTransactionsModule(this IServiceCollection services)
    {
        services.AddDateTimeProvider();

        services
            .AddDataAccess<TransactionsWriteDbContext, TransactionsWriteDbContextConfigurator>()
            .AddDataAccess<TransactionsReadDbContext, TransactionsReadDbContextConfigurator>();

        services
            .AddHandlersFromAssembly(Assembly.GetAssembly(typeof(TransactionsModuleExtensions)));

        return services;
    }
}