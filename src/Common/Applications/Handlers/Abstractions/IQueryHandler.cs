using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Applications.Handlers.Abstractions;

/// <summary>
/// Query handler.
/// </summary>
/// <typeparam name="TQuery">Type of query.</typeparam>
/// <typeparam name="TResponse">Type of request result.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
{
    /// <summary>
    /// Handles the query.
    /// </summary>
    /// <param name="query">Query.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns></returns>
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}