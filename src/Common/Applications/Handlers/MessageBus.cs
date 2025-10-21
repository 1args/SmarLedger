using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Applications.Handlers;

/// <summary>
/// Сlass that represents the bus for sending commands and queries to their handlers.
/// </summary>
public sealed class MessageBus(IServiceProvider serviceProvider) : IMessageBus
{
    /// <inheritdoc />
    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"No handler found for command type {command.GetType().Name}");
        }

        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResponse>, TResponse>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException($"No HandleAsync method found for handler type {handlerType.Name}");
        }

        var task = (Task<TResponse>)method.Invoke(handler, new object[] { command, cancellationToken })!;
        return await task;
    }

    /// <inheritdoc />
    public async Task SendAsync(ICommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var handler = serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"No handler found for command type {command.GetType().Name}");
        }

        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException($"No HandleAsync method found for handler type {handlerType.Name}");
        }

        var task = (Task)method.Invoke(handler, new object[] { command, cancellationToken })!;
        await task;
    }

    /// <inheritdoc />
    public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"No handler found for query type {query.GetType().Name}");
        }

        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResponse>, TResponse>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException($"No HandleAsync method found for handler type {handlerType.Name}");
        }

        var task = (Task<TResponse>)method.Invoke(handler, new object[] { query, cancellationToken })!;
        return await task;
    }
}