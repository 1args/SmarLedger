using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Applications.Handlers.Abstractions;

/// <summary>
/// Defines a message bus for sending commands and queries to their handlers.
/// </summary>
public interface IMessageBus
{
    /// <summary>
    /// Sends a command that expects a response to its registered handler.
    /// </summary>
    /// <typeparam name="TResponse">Type of the response returned by the command handler.</typeparam>
    /// <param name="command">Command./>.
    /// </param><param name="cancellationToken">Token to cancel the operation.</param> 
    /// <returns>Result produced by the command handler.</returns>
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a command to its registered handler.
    /// </summary>
    /// <param name="command">Command.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SendAsync(ICommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a query to its corresponding handler and returns a response.
    /// </summary>
    /// <typeparam name="TResponse">Type of the response expected from the query handler.</typeparam>
    /// <param name="query">Query.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Result from the query handler.</returns>
    Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);
}