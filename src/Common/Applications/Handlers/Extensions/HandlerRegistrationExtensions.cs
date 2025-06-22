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
            .AddCommandAndQueryHandlersFromAssembly(assembly)
            .AddEventConsumersFromAssembly(assembly);

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
            typeof(IQueryHandler<,>),
            typeof(ICommandHandler<,>),
            typeof(ICommandHandler<>)
        };

        var handlerTypes = assembly.DefinedTypes
            .Where(t => t.IsConcrete() && !t.IsOpenGenericType())
            .Select(t => new
            {
                Type = t.AsType(),
                Interfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Select(i => i.GetGenericTypeDefinition())
                    .Intersect(handlerInterfaces)
                    .ToList()
            });

        foreach (var handler in handlerTypes)
        {
            foreach (var interfaceType in handler.Interfaces)
            {
                var concreteInterface = handler.Type.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);

                services.TryAddScoped(concreteInterface, handler.Type);
            }
        }
        
        return services;
    }

    /// <summary>
    /// Registers all event consumers found in the given assembly.
    /// Also registers them with MassTransit as <see cref="IConsumer{T}"/>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="assembly">Assembly to scan.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddEventConsumersFromAssembly(this IServiceCollection services, Assembly? assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        var eventConsumerInterface = typeof(IEventConsumer<>);

        var eventConsumerTypes = assembly.GetTypes()
            .Where(t => t.IsConcrete() && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == eventConsumerInterface));

        foreach (var eventConsumerType in eventConsumerTypes)
        {
            var concreteInterface = eventConsumerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == eventConsumerInterface);

            services.AddScoped(concreteInterface, eventConsumerType);

            services.AddScoped(typeof(IConsumer<>).MakeGenericType(
                concreteInterface.GetGenericArguments()[0]),
                eventConsumerType);
        }

        return services;
    }
}