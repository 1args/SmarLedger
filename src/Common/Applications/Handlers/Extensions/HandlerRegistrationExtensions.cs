using System.Reflection;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartLedger.Common.Applications.Handlers.Abstractions;

namespace SmartLedger.Common.Applications.Handlers.Extensions;

/// <summary>
/// Extension for registering handlers, validators, and event consumers from a given assembly.
/// </summary>
public static class HandlerRegistrationExtensions
{
    /// <summary>
    /// Registers validators, command/query handlers, and event consumers from the specified assembly.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="assembly">Assembly to scan.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly? assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        services
            .AddValidatorsFromAssembly(assembly)
            .AddCommandAndQueryHandlersFromAssembly(assembly);

        return services;
    }

    /// <summary>
    /// Registers all FluentValidation validators found in the given assembly.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="assembly">Assembly to scan.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddValidatorsFromAssembly(this IServiceCollection services, Assembly? assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Transient);

        return services;
    }

    /// <summary>
    /// Registers all command and query handlers found in the given assembly.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="assembly">Assembly to scan.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddCommandAndQueryHandlersFromAssembly(this IServiceCollection services, Assembly? assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        var handlerInterfaces = new[]
        {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
        };

        var handlerTypes = assembly.DefinedTypes
            .Where(t => t.IsConcrete() && !t.IsOpenGenericType())
            .Where(t => Array.Exists(
                t.GetInterfaces(),
                i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition())))
            .ToList();

        foreach (var handlerType in handlerTypes)
        { 
            var interfaces = handlerType.GetInterfaces()
               .Where(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition()));
            
            foreach (var @interface in interfaces)
            {
               services.TryAddScoped(@interface, handlerType);
            }
        }

        return services;
    }
}