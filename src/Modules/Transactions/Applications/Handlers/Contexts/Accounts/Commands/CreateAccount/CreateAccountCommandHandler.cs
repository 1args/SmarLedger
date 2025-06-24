using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;

/// <summary>
/// Handles the logic for processing <see cref="CreateAccountCommand"/>.
/// </summary>
public sealed class CreateAccountCommandHandler(
    IAccountService accountService,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateAccountCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var request = new CreateAccountModel(
            command.Name,
            command.UserId,
            dateTimeProvider.UtcNow);

        await accountService.CreateAsync(request, cancellationToken);
    }
}