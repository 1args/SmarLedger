using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Infrastructure.Abstractions;
using System.Linq.Expressions;

namespace SmartLedger.Common.Infrastructure.Repositories;

/// <inheritdoc/>
public sealed class Repository<TEntity, TDbContext> :
    IRepository<TEntity, TDbContext>
    where TEntity : class
    where TDbContext : DbContext
{
    /// <summary>
    /// Database context.
    /// </summary>
    private TDbContext DbContext { get; }

    /// <summary>
    /// Storage of entities./>
    /// </summary>
    private DbSet<TEntity> DbSet { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity, TDbContext}"/> class.
    /// </summary>
    /// <param name="dbContext">DbContext.</param>
    public Repository(TDbContext dbContext)
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
    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbContext.Remove(entity);

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