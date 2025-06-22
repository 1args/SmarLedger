using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Applications.Handlers.Abstractions;

/// <summary>
/// Command handler with no result.
/// </summary>
/// <typeparam name="TCommand">Command type.</typeparam>
public interface ICommandHandler<in TCommand> 
    where TCommand : class, ICommand
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Command handler with result.
/// </summary>
/// <typeparam name="TCommand">Command type.</typeparam>
/// <typeparam name="TResponse">Command result type.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : class, ICommand<TResponse>
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command">Command type.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <typeparam name="TResponse">Command result type.</typeparam>
    /// <returns>TResponse</returns>
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}