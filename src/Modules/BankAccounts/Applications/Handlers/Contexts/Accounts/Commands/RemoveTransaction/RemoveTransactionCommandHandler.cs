using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Domain.Enums;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Events.TransactionRemoved;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;

/// <summary>
/// Handles the logic for processing <see cref="RemoveTransactionCommand"/>.
/// </summary>
public sealed class RemoveTransactionCommandHandler(
    IAccountsService accountService,
    IAccountsRetrievalService accountsRetrievalService,
    IEventBus eventBus) : ICommandHandler<RemoveTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RemoveTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new TransactionRemovalModel(
            command.AccountId,
            command.TransactionId);

        await accountService.RemoveTransactionAsync(request, cancellationToken);

        var getRequest = new GetTransactionModel(command.AccountId, command.TransactionId);
        var transaction = await accountsRetrievalService.GetTransactionAsync(
            getRequest, cancellationToken);

        var boundedEvent = new TransactionRemovedEvent(request.TransactionId);

        var integrationEvent = new TransactionRemovedIntegrationEvent(
            transaction.UserId,
            transaction.Amount,
            Enum.Parse<TransactionType>(transaction.Type),
            Enum.Parse<FinancialCategory>(transaction.Category),
            transaction.CreatedAt);

        await Task.WhenAll(
            eventBus.PublishAsync(boundedEvent, cancellationToken),
            eventBus.PublishAsync(integrationEvent, cancellationToken));
    }
}