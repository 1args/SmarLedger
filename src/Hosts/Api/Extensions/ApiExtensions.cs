using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Cqrs.Extensions;
using SmartLedger.Common.Hosts.Features;
using SmartLedger.Common.Infrastructure.Events;
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
            .AddConfiguredMessageBroker(configuration)
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
    /// Registers authentication rules using JWT Bearer authentication with Keycloak as the identity provider.
    /// </summary>
    private static IServiceCollection AddAuthenticationViaKeycloak(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeycloakAuthorizationOptions>(configuration.GetSection(nameof(KeycloakAuthorizationOptions)));

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
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated successfully");
                        return Task.CompletedTask;
                    }
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

        services.AddScoped(provider => new Lazy<IAuthorizationData>(provider.GetRequiredService<IAuthorizationData>));

        services.AddScoped<IAuthorizationData, AuthorizationData>();

        return services;
    }
}