using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Events.Events;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;

/// <summary>
/// Handles the logic for processing <see cref="CreateAccountCommand"/>.
/// </summary>
public sealed class CreateAccountCommandHandler(
    IAccountService accountService,
    IDateTimeProvider dateTimeProvider,
    IEventBus eventBus) : ICommandHandler<CreateAccountCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var request = new CreateAccountModel(
            command.Name,
            command.UserId,
            dateTimeProvider.UtcNow);

        var accountId = await accountService.CreateAsync(request, cancellationToken);

        await eventBus.PublishAsync(
            new AccountCreatedEvent(accountId, request.Name, request.UserId, request.CreatedAt), 
            cancellationToken);
    }
}