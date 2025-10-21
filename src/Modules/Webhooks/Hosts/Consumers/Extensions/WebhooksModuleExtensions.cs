using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Infrastructures.DataAccess.Extensions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Abstractions;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Dispatchers;

namespace SmartLedger.Modules.Webhooks.Host.Consumers.Extensions;

/// <summary>
/// Extensions for registering the Webhooks module components.
/// </summary>
public static class WebhooksModuleExtensions
{
    /// <summary>
    /// Registers the Webhooks module components.
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
            .AddDataAccess<WebhooksDbContext, WebhooksDbContextConfigurator>()
            .AddScoped<IWebhooksDispatcher, WebhooksDispatcher>();

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IWebhooksService, WebhooksService>();

        return services;
    }
}