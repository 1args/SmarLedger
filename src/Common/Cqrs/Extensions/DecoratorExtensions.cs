using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Cqrs.Decorators;

namespace SmartLedger.Common.Cqrs.Extensions;

/// <summary>
/// Extension for registering handler decorators.
/// </summary>
public static class DecoratorExtensions
{
    /// <summary>
    /// Registers decorators for command and query handlers.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddRequestValidationDecorators(this IServiceCollection services)
    {
        // "Decorate" method requires the addition of the "Scrutor" library.
        services
            .Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandHandler<>))
            .Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
            //.Decorate(typeof(IQueryHandler<,>), typeof(ValidationDecorator.QueryHandler<,>));

        return services;
    }

    /// <summary>
    /// Registers transaction decorators for command handlers that require transactions.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext type.</typeparam>
    /// <param name="services">Service collection.</param>
    /// <returns>Modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddTransactionDecorators<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.Decorate(typeof(ICommandHandler<>), (handler, provider) =>
        {
            var dbContext = provider.GetRequiredService<TDbContext>();
            var decoratorType = typeof(TransactionDecorator.CommandHandler<>);
            var genericType = handler.GetType().GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .GetGenericArguments()[0];

            var concreteDecoratorType = decoratorType.MakeGenericType(genericType);
            return ActivatorUtilities.CreateInstance(provider, concreteDecoratorType, handler, dbContext);
        });

        services.Decorate(typeof(ICommandHandler<,>), (handler, provider) =>
        {
            var dbContext = provider.GetRequiredService<TDbContext>();
            var decoratorType = typeof(TransactionDecorator.CommandHandler<,>);
            var handlerInterface = handler.GetType().GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            var genericArgs = handlerInterface.GetGenericArguments();
            var concreteDecoratorType = decoratorType.MakeGenericType(genericArgs[0], genericArgs[1]);
            return ActivatorUtilities.CreateInstance(provider, concreteDecoratorType, handler, dbContext);
        });

        return services;
    }
}