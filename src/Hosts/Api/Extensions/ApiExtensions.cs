using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Minio;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Cqrs.Extensions;
using SmartLedger.Common.Hosts.Features;
using SmartLedger.Common.Infrastructures.DataAccess.Events;
using SmartLedger.Common.Infrastructures.FileStorage;
using SmartLedger.Common.Infrastructures.FileStorage.Abstractions;
using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Contracts.Options;

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
            .ConfigureOptions(configuration)
            .AddConfiguredMessageBroker(configuration)
            .AddMinioFileStorage(configuration)
            .AddHttpClient()
            .AddAuthenticationViaKeycloak(configuration)
            .AddControllers();

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
    /// Configures options for various services using the provided configuration section.
    /// </summary>
    private static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)))
            .Configure<KeycloakAuthorizationOptions>(configuration.GetSection(nameof(KeycloakAuthorizationOptions)))
            .Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)))
            .Configure<HybridCacheOptions>(configuration.GetSection(nameof(HybridCacheOptions)));

        return services;
    }

    /// <summary>
    /// Registers the message broker with MassTransit and configures it.
    /// </summary>
    private static IServiceCollection AddConfiguredMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionString}' could not be found for message broker.");
        }

        services.AddMessageBroker<OutboxDbContext>(connectionString);

        return services;
    }

    /// <summary>
    /// Registers authentication rules using JWT Bearer authentication with Keycloak as the identity provider.
    /// </summary>
    private static IServiceCollection AddAuthenticationViaKeycloak(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakOptions = configuration.GetSection(nameof(KeycloakAuthorizationOptions)).Get<KeycloakAuthorizationOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.MetadataAddress = keycloakOptions!.MetadataAddress;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = keycloakOptions.TokenValidationOptions.ValidateIssuerSigningKey,
                    ValidateIssuer = keycloakOptions.TokenValidationOptions.ValidateIssuer,
                    ValidIssuer = keycloakOptions.TokenValidationOptions.Issuer,
                    ValidateAudience = keycloakOptions.TokenValidationOptions.ValidateAudience,
                    ValidateLifetime = true,
                    ClockSkew = keycloakOptions.TokenValidationOptions.ClockSkew
                };
            });

        services.AddAuthorization();
        services.AddTransient<KeycloakAdminAuthHandler>();

        services.AddHttpClient<IKeycloakGeneratedApiClient, KeycloakGeneratedApiClient>(client =>
        {
            client.BaseAddress = new Uri(keycloakOptions!.AdminBaseUrl);
        }).AddHttpMessageHandler<KeycloakAdminAuthHandler>();

        services
            .AddScoped<IKeycloakAuthorizationApiClient, KeycloakAuthorizationApiClient>()
            .AddScoped<IKeycloakUserApiClient, KeycloakUserApiClient>();

        services
            .AddScoped(provider => new Lazy<IAuthorizationData>(provider.GetRequiredService<IAuthorizationData>))
            .AddScoped<IAuthorizationData, AuthorizationData>();

        return services;
    }

    /// <summary>
    /// Registers MinIO file storage.
    /// </summary>
    private static IServiceCollection AddMinioFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMinioClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.UseSsl)
                .Build();
        });

        services.AddScoped<IMinioFileStorage, MinioFileStorage>();

        return services;
    }
}