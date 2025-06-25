using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;

/// <summary>
/// Handles the logic for processing <see cref="AddTransactionCommand"/>.
/// </summary>
public sealed class AddTransactionCommandHandler(
    IAccountService accountService,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<AddTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(AddTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new AddTransactionModel(
            command.AccountId,
            command.Amount,
            command.Type,
            command.Category,
            dateTimeProvider.UtcNow,
            command.Notes);

        await accountService.AddTransactionAsync(request, cancellationToken);
    }
}