using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.DeleteAccount;

/// <summary>
/// Handles the logic for processing <see cref="DeleteAccountCommand"/>.
/// </summary>
public sealed class DeleteAccountCommandHandler(
    IAccountsService accountService,
    IEventBus bus) : ICommandHandler<DeleteAccountCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var request = new IdOnlyModel(command.AccountId);

        await accountService.DeleteAccountAsync(request, cancellationToken);

        await bus.PublishAsync(
            new AccountDeletedEvent(request.AccountId), 
            cancellationToken);
    }
}