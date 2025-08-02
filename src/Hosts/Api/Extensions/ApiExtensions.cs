using System.Reflection;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Cqrs.Extensions;
using SmartLedger.Common.Hosts.Features;
using SmartLedger.Common.Infrastructure.Events;

namespace SmartLedger.Hosts.Api.Extensions;

/// <summary>
/// Extensions for registering API services and configurations.
/// </summary>
public static class ApiExtensions
{
    /// <summary>
    /// Registers API services and configurations.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOpenApi()
            .AddDateTimeProvider()
            .AddConfiguredMessageBroker(configuration)
            .AddHttpClient();

        var featureRegistry = new FeaturesRegistry()
            .RegisterFeaturesFromAssembly(Assembly.GetExecutingAssembly());

        featureRegistry.ApplyFeatures(services, configuration);

        return services;
    }

    /// <summary>
    /// Registers decorators.
    /// </summary>
    public static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services
            .AddRequestValidationDecorators()
            .AddTransactionDecorators<OutboxDbContext>();

        return services;
    }

    /// <summary>
    /// Registers the message broker with MassTransit and configures it.
    /// </summary>
    private static IServiceCollection AddConfiguredMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionString}' could not be found for message broker.");
        }

        services.AddMessageBroker<OutboxDbContext>(connectionString);

        return services;
    }
}