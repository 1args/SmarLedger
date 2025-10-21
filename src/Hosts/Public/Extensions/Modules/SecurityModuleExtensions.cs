using SmartLedger.Common.Applications.Handlers.Extensions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Abstractions;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Login;

namespace SmartLedger.Hosts.Public.Extensions.Modules;

/// <summary>
/// Extensions for registering the Budgets module components.
/// </summary>
public static class SecurityModuleExtensions
{
    /// <summary>
    /// Registers the Budgets module.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSecurityModule(this IServiceCollection services)
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
            .AddScoped<IAuthorizationService, AuthorizationService>()
            .AddScoped<IUsersService, UsersService>();

        services
            .AddHandlersFromAssembly(typeof(LoginCommandHandler).Assembly);

        return services;
    }
}