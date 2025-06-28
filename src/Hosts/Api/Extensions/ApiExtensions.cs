using Microsoft.AspNetCore.Http.Features;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Cqrs.Extensions;
using SmartLedger.Common.Infrastructure.Events;
using SmartLedger.Common.Infrastructure.Extensions;
using SmartLedger.Hosts.Api.ExceptionHandling;

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
            .AddGlobalExceptionHandler()
            .AddConfiguredMessageBroker(configuration)
            .AddDecorators();

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

    /// <summary>
    /// Registers a global exception handler and configures ProblemDetails for consistent error responses.
    /// </summary>
    private static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}