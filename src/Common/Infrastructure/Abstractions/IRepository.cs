using System.Linq.Expressions;

namespace SmartLedger.Common.Infrastructure.Abstractions;

/// <summary>
/// Generic repository for data access operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being managed.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Returns a non-material collection of entities.
    /// </summary>
    /// <returns>Non-material collection of entities.</returns>
    IQueryable<TEntity> AsQueryable();

    /// <summary>
    /// Adds a new entity to the data source.
    /// </summary>
    /// <param name="entity">Entity.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing entity in the data source.
    /// </summary>
    /// <param name="entity">Entity.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a collection of entities from the data source.
    /// </summary>
    /// <param name="entities">Entities.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdateRangeAsync(TEntity[] entities, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a collection of entities from the data source.
    /// </summary>
    /// <param name="entity">Entity.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Filters the entities based on the specified predicate expression.
    /// </summary>
    /// <param name="expression">Expression used to filter the entities.</param>
    /// <returns>Non-materialised collection of filtered entities.</returns>
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// Executes a raw SQL command against the data source.
    /// </summary>
    /// <param name="sql">Sql command.</param>
    /// <param name="parameters">Parameters.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ExecuteRawSqlAsync(string sql, IReadOnlyCollection<object> parameters, CancellationToken cancellationToken);
}