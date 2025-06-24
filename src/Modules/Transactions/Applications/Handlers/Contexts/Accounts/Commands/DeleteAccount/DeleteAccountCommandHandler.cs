using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.DeleteAccount;

/// <summary>
/// Handles the logic for processing <see cref="DeleteAccountCommand"/>.
/// </summary>
public sealed class DeleteAccountCommandHandler(
    IAccountService accountService) : ICommandHandler<DeleteAccountCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var request = new IdOnlyModel(command.AccountId);

        await accountService.DeleteAsync(request, cancellationToken);
    }
}