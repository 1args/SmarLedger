using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Contracts.Events;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.CreateTransaction;

/// <summary>
/// Handles the logic for processing <see cref="CreateTransactionCommand"/>.
/// </summary>
public sealed class CreateTransactionCommandHandler(
    IAccountsService accountService,
    IDateTimeProvider dateTimeProvider,
    IEventBus bus) : ICommandHandler<CreateTransactionCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Guid> HandleAsync(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new TransactionCreationModel(
            command.AccountId,
            command.Amount,
            command.Type,
            command.Category,
            dateTimeProvider.UtcNow,
            command.Notes);

        var response = await accountService.CreateTransactionAsync(request, cancellationToken);

        await bus.PublishAsync(
            new TransactionCreatedEvent(
                response.TransactionId,
                request.AccountId,
                response.UserId,
                request.Amount,
                request.Type,
                request.Category,
                request.CreatedAt,
                request.Notes),
            cancellationToken);

        return response.TransactionId;
    }
}