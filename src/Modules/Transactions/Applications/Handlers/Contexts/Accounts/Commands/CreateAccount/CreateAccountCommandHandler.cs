using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events.AccountCreated;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;

/// <summary>
/// Handles the logic for processing <see cref="CreateAccountCommand"/>.
/// </summary>
public sealed class CreateAccountCommandHandler(
    IAccountsService accountService,
    IDateTimeProvider dateTimeProvider,
    IEventBus eventBus) : ICommandHandler<CreateAccountCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var request = new AccountCreationModel(
            command.Name,
            dateTimeProvider.UtcNow);

        var response = await accountService.CreateAsync(request, cancellationToken);

        await eventBus.PublishAsync(
            new AccountCreatedEvent(response.AccountId, request.Name, response.UserId, request.CreatedAt), 
            cancellationToken);

        return response.AccountId;
    }
}