using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Cqrs.Decorators;

/// <summary>
/// Transaction decorators for command handlers.
/// Wraps command execution in a database transaction.
/// </summary>
internal sealed class TransactionDecorator
{
    /// <summary>
    /// Decorator for command handlers without result.
    /// </summary>
    internal sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> commandHandler,
        DbContext dbContext) : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await commandHandler.HandleAsync(command, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    /// <summary>
    /// Decorator for command handlers with result.
    /// </summary>
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> commandHandler,
        DbContext dbContext) : ICommandHandler<TCommand, TResponse>
        where TCommand : class, ICommand<TResponse>
    {
        public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await commandHandler.HandleAsync(command, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
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
}