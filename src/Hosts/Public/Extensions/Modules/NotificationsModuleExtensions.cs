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
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services)
    {
        services
            .AddApplications();

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
}