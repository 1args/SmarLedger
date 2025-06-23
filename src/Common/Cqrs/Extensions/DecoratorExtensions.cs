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
    public static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        // "Decorate" method requires the addition of the "Scrutor" library.
        services
            .Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandHandler<>))
            .Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>))
            .Decorate(typeof(IQueryHandler<,>), typeof(ValidationDecorator.QueryHandler<,>));

        return services;
    }
}