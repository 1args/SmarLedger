using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events.TransactionRemoved;
using SmartLedger.Modules.Transactions.Contracts.Events;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;

/// <summary>
/// Handles the logic for processing <see cref="RemoveTransactionCommand"/>.
/// </summary>
public sealed class RemoveTransactionCommandHandler(
    IAccountsService accountService,
    ITransactionsRetrievalService transactionsRetrievalService,
    IEventBus eventBus) : ICommandHandler<RemoveTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RemoveTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new TransactionRemovalModel(
            command.AccountId,
            command.TransactionId);

        await accountService.RemoveTransactionAsync(request, cancellationToken);

        var transaction = await transactionsRetrievalService.GetTransactionAsync(
            command.TransactionId, cancellationToken);

        var boundedEvent = new TransactionRemovedEvent(request.TransactionId);

        var integrationEvent = new TransactionRemovedIntegrationEvent(
            transaction.UserId,
            transaction.Amount,
            Enum.Parse<TransactionType>(transaction.Type),
            Enum.Parse<TransactionCategory>(transaction.Category),
            transaction.CreatedAt);

        await Task.WhenAll(
            eventBus.PublishAsync(boundedEvent, cancellationToken),
            eventBus.PublishAsync(integrationEvent, cancellationToken));
    }
}