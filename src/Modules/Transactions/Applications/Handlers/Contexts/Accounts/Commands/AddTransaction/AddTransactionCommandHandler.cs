using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events.TransactionAdded;
using SmartLedger.Modules.Transactions.Contracts.Events;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;

/// <summary>
/// Handles the logic for processing <see cref="AddTransactionCommand"/>.
/// </summary>
public sealed class AddTransactionCommandHandler(
    IAccountsService accountService,
    IDateTimeProvider dateTimeProvider,
    IEventBus eventBus) : ICommandHandler<AddTransactionCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> HandleAsync(AddTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new TransactionAdditionModel(
            command.AccountId,
            command.Amount,
            command.Type,
            command.Category,
            dateTimeProvider.UtcNow,
            command.Notes);

        var response = await accountService.AddTransactionAsync(request, cancellationToken);

        var boundedEvent = new TransactionAddedEvent(
            response.TransactionId,
            request.AccountId,
            response.UserId,
            request.Amount,
            request.Type,
            request.Category,
            request.CreatedAt,
            request.Notes);

        var integrationEvent = new TransactionAddedIntegrationEvent(
            response.UserId,
            request.Amount,
            request.Type,
            request.Category,
            request.CreatedAt);

        await Task.WhenAll(
            eventBus.PublishAsync(boundedEvent, cancellationToken),
            eventBus.PublishAsync(integrationEvent, cancellationToken));

        return response.TransactionId;
    }
} 