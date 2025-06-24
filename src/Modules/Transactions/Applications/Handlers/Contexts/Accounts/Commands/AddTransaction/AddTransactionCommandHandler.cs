using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;

/// <summary>
/// Handles the logic for processing <see cref="AddTransactionCommand"/>.
/// </summary>
public sealed class AddTransactionCommandHandler(
    IAccountService accountService) : ICommandHandler<AddTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(AddTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new AddRemoveTransactionModel(
            command.AccountId,
            command.Amount,
            command.Type);

        await accountService.AddTransactionAsync(request, cancellationToken);
    }
}