using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Infrastructure.Abstractions;
using System.Linq.Expressions;

namespace SmartLedger.Common.Infrastructure.Repositories;

/// <inheritdoc/>
public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DbContext DbContext { get; }

    /// <summary>
    /// Storage of entities./>
    /// </summary>
    private DbSet<TEntity> DbSet { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
    /// </summary>
    /// <param name="dbContext">DbContext.</param>
    public Repository(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<TEntity>();
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> AsQueryable()
    {
        return DbSet.AsQueryable();
    }

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await DbContext.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var state = DbContext.Entry(entity).State;

        if (state == EntityState.Detached)
        {
            DbContext.Attach(entity);
        }

        DbContext.Update(entity);
        return SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Attach(entity);
            }
            DbContext.Update(entity);
        }
        return SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            DbContext.Remove(entity);
        }

        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return DbSet.Where(expression);
    }

    /// <inheritdoc/>
    public Task ExecuteRawSqlAsync(string sql, IReadOnlyCollection<object> parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sql);

        DbContext.Database.SqlQueryRaw<TEntity>(sql, parameters);

        return SaveChangesAsync(cancellationToken);
    }

    private Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}