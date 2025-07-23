using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Serilog;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Cqrs.Extensions;
using SmartLedger.Common.Infrastructure.Events;
using SmartLedger.Hosts.Api.ExceptionHandling;
using HybridCacheOptions = SmartLedger.Common.Contracts.Options.HybridCacheOptions;

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
            .AddLogging(configuration)
            .AddGlobalExceptionHandler()
            .AddDateTimeProvider()
            .AddConfiguredMessageBroker(configuration)
            .AddCaching(configuration);

        return services;
    }

    /// <summary>
    /// Registers Serilog.
    /// </summary>
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(logger, dispose: true);
        });

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

    /// <summary>
    /// Adds caching.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var hybridCacheOptions = configuration.GetSection(nameof(HybridCacheOptions)).Get<HybridCacheOptions>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
        });

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = hybridCacheOptions!.MaximumPayloadBytes;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = hybridCacheOptions.LocalCacheExpirationSeconds,
                Expiration = hybridCacheOptions.DefaultExpirationSeconds
            };
        });

        return services;
    }
}