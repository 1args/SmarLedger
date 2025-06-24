using FluentValidation;
using FluentValidation.Results;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Common.Cqrs.Decorators;

/// <summary>
/// Contains validation decorators for command and query handlers.
/// These wrappers perform input validation before delegating to the actual handler.
/// </summary>
internal sealed class ValidationDecorator
{
    /// <summary>
    /// Decorator for command handlers without result.
    /// </summary>
    internal sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> commandHandler,
        IEnumerable<IValidator<TCommand>> validators) : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            var validationFailures = await ValidateAsync(command, validators, cancellationToken);

            if (validationFailures.Count != 0)
            {
                throw new ValidationException(BuildErrorMessage(validationFailures), validationFailures);
            }

            await commandHandler.HandleAsync(command, cancellationToken);
        }
    }

    /// <summary>
    /// Decorator for command handlers with result.
    /// </summary>
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> commandHandler,
        IEnumerable<IValidator<TCommand>> validators) : ICommandHandler<TCommand, TResponse>
        where TCommand : class, ICommand<TResponse>
    {
        public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            var validationFailures = await ValidateAsync(command, validators, cancellationToken);

            if (validationFailures.Count != 0)
            {
                throw new ValidationException(BuildErrorMessage(validationFailures), validationFailures);
            }

            return await commandHandler.HandleAsync(command, cancellationToken);
        }
    }

    /// <summary>
    /// Decorator for query handlers.
    /// </summary>
    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> queryHandler,
        IEnumerable<IValidator<TQuery>> validators) : IQueryHandler<TQuery, TResponse>
        where TQuery : class, IQuery<TResponse>
    {
        public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
        {
            var validationFailures = await ValidateAsync(query, validators, cancellationToken);

            if (validationFailures.Count != 0)
            {
                throw new ValidationException(BuildErrorMessage(validationFailures), validationFailures);
            }

            return await queryHandler.HandleAsync(query, cancellationToken);
        }
    }

    /// <summary>
    /// Validates the request using the provided validators.
    /// </summary>
    private static async Task<List<ValidationFailure>> ValidateAsync<TRequest>(
        TRequest request,
        IEnumerable<IValidator<TRequest>> validators,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var tasks = validators
            .Select(validator => validator.ValidateAsync(context, cancellationToken));

        var validationResults = await Task.WhenAll(tasks);

        var validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationFailure(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToList();

        return validationFailures;
    }

    /// <summary>
    /// Builds a readable validation error message from the list of failures.
    /// </summary>
    private static string BuildErrorMessage(IEnumerable<ValidationFailure> failures)
    {
        return string.Join(
            Environment.NewLine,
            failures.Select(f => $"Field {f.PropertyName}: {f.ErrorMessage}"));
    }
}