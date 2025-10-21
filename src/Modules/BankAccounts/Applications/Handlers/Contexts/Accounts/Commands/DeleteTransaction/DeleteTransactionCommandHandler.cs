using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Domain.Enums;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.DeleteTransaction;

/// <summary>
/// Handles the logic for processing <see cref="DeleteTransactionCommand"/>.
/// </summary>
public sealed class DeleteTransactionCommandHandler(
    IAccountsService accountService,
    IAccountsRetrievalService accountsRetrievalService,
    IEventBus bus) : ICommandHandler<DeleteTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        var deletionRequest = new TransactionDeletionModel(
            command.AccountId,
            command.TransactionId);

        await accountService.DeleteTransactionAsync(deletionRequest, cancellationToken);

        var retrievalRequest = new GetTransactionModel(command.AccountId, command.TransactionId);

        var transaction = await accountsRetrievalService.GetTransactionAsync(
            retrievalRequest, cancellationToken);

        await bus.PublishAsync(
            new TransactionDeletedEvent(
                deletionRequest.TransactionId,
                transaction.UserId,
                transaction.Amount,
                Enum.Parse<TransactionType>(transaction.Type),
                Enum.Parse<FinancialCategory>(transaction.Category),
                transaction.CreatedAt), 
            cancellationToken);
    }
}