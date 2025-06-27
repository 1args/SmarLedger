using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;

/// <summary>
/// Handles the logic for processing <see cref="RemoveTransactionCommand"/>.
/// </summary>
public sealed class RemoveTransactionCommandHandler(
    IAccountsService accountService,
    IEventBus eventBus) : ICommandHandler<RemoveTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RemoveTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new TransactionRemovalModel(
            command.AccountId,
            command.TransactionId);

        await accountService.RemoveTransactionAsync(request, cancellationToken);

        await eventBus.PublishAsync(
            new TransactionRemovedEvent(request.AccountId, request.TransactionId), 
            cancellationToken);
    }
}