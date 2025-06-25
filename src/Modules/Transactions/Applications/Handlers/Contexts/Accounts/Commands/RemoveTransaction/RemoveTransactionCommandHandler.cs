using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;

/// <summary>
/// Handles the logic for processing <see cref="RemoveTransactionCommand"/>.
/// </summary>
public sealed class RemoveTransactionCommandHandler(
    IAccountService accountService) : ICommandHandler<RemoveTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(RemoveTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new RemoveTransactionModel(
            command.AccountId,
            command.TransactionId);

        await accountService.RemoveTransactionAsync(request, cancellationToken);
    }
}