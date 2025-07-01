using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Contracts.Events;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateTransactionAmount;

/// <summary>
/// Handles the logic for processing <see cref="UpdateTransactionAmountCommand"/>.
/// </summary>
public sealed class UpdateTransactionAmountCommandHandler(
    ITransactionsService transactionService,
    IDateTimeProvider dateTimeProvider,
    IEventBus eventBus) : ICommandHandler<UpdateTransactionAmountCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(UpdateTransactionAmountCommand command, CancellationToken cancellationToken)
    {
        var request = new UpdateAmountModel(
            command.TransactionId,
            command.NewAmount,
            dateTimeProvider.UtcNow);

        await transactionService.UpdateAmountAsync(request, cancellationToken);

        await eventBus.PublishAsync(
            new TransactionAmountUpdatedEvent(command.TransactionId, command.NewAmount, request.UpdatedAt),
            cancellationToken);
    }
}