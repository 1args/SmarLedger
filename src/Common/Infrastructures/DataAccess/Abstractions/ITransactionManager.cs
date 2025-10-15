using System.Data;

namespace SmartLedger.Common.Infrastructures.DataAccess.Abstractions;

/// <summary>
/// Interface for managing transactions.
/// </summary>
public interface ITransactionManager
{
    /// <summary>
    /// Starts a transaction with the specified action and isolation level.
    /// </summary>
    /// <param name="action">Operation to execute.</param>
    /// <param name="isolationLevel">Isolation level.</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns></returns>
    Task StartEffectAsync(Func<CancellationToken, Task> action, IsolationLevel isolationLevel,
        CancellationToken cancellationToken);

    /// <summary>
    /// Starts a transaction with the specified action and isolation level, returning a result.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="action">Operation to execute.</param>
    /// <param name="isolationLevel">Isolation level.</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns>Operation result.</returns>
    Task<TResult> StartEffectAsync<TResult>(Func<CancellationToken, Task<TResult>> action, IsolationLevel isolationLevel,
        CancellationToken cancellationToken);
}