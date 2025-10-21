using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Factories;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Factories.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Services;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Services.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Notifications.Services;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Notifications.Services.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Templates.Services;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Templates.Services.Abstractions;
using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;

namespace SmartLedger.Hosts.Public.Extensions.Modules;

/// <summary>
/// Extensions for registering the Budgets module components.
/// </summary>
public static class NotificationsModuleExtensions
{
    /// <summary>
    /// Registers the Budgets module components.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    /// <param name="configuration">Configuration.</param>
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApplications()
            .AddRequiredServices(configuration);

        return services;
    }

    /// <summary>
    /// Registers applications components.
    /// </summary>
    private static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services
            .AddScoped<IEmailSendingStrategiesFactory, EmailSendingStrategiesFactory>()
            .AddScoped<IEmailSendingService, EmailSendingService>()
            .AddScoped<IEmailSendingStrategy, SmtpEmailSendingStrategy>()
            .AddScoped<ITemplateProvider, TemplateProvider>()
            .AddScoped<INotificationService, NotificationService>();

        return services;
    }

    /// <summary>
    /// Adds the required services for consumers.
    /// </summary>
    private static IServiceCollection AddRequiredServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<KeycloakAdminAuthHandler>();

        services.AddHttpClient<IKeycloakGeneratedApiClient, KeycloakGeneratedApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Keycloak:AdminBaseUrl"] ?? "http://localhost:8090");
        }).AddHttpMessageHandler<KeycloakAdminAuthHandler>();

        services
            .AddScoped<IKeycloakUserApiClient, KeycloakUserApiClient>();

        return services;
    }
}