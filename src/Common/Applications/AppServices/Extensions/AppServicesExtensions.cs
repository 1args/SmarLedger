using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider;
using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;

namespace SmartLedger.Common.Applications.AppServices.Extensions;

/// <summary>
/// Extension for registering application services.
/// </summary>
public static class AppServicesExtensions
{
    /// <summary>
    /// Registers the date time provider.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}