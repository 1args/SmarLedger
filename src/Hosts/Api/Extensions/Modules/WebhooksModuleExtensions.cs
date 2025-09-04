using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.Webhooks.Applications.AppServices;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.Handlers.Commands.CreateWebhookSubscription;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;

namespace SmartLedger.Hosts.Api.Extensions.Modules;

/// <summary>
/// Extensions for registering the Webhooks module.
/// </summary>
public static class WebhooksModuleExtensions
{
    /// <summary>
    /// Registers the Webhooks module.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddWebhooksModule(this IServiceCollection services)
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
            .AddDataAccess<WebhooksDbContext, WebhooksDbContextConfigurator>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IWebhooksService, WebhooksService>()
            .AddScoped<IWebhooksDispatcher, WebhooksDispatcher>();

        services
            .AddHandlersFromAssembly(typeof(CreateWebhookSubscriptionCommand).Assembly);

        return services;
    }
}