using System.Data;
using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;

namespace SmartLedger.Common.Infrastructures.DataAccess.Transactions;

/// <summary>
/// Transaction manager for handling database transactions.
/// </summary>
/// <param name="dbContext"></param>
public sealed class TransactionManager(DbContext dbContext) : ITransactionManager
{
    /// <summary>
    /// Database context.
    /// </summary>
    public DbContext DbContext { get; } = dbContext;

    /// <inheritdoc />
    public async Task StartEffect(Func<CancellationToken, Task> action, IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await action(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
           await transaction.RollbackAsync(cancellationToken);
           throw;
        }
    }

    /// <inheritdoc />
    public async Task<TResult> StartEffect<TResult>(Func<CancellationToken, Task<TResult>> action, IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}