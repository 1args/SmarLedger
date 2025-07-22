using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Events.TransactionCategorized;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.CategorizeTransaction;

/// <summary>
/// Handles the logic for processing <see cref="CategorizeTransactionCommand"/>.
/// </summary>
public sealed class CategorizeTransactionCommandHandler(
    ITransactionsService transactionService,
    IDateTimeProvider dateTimeProvider,
    IEventBus eventBus) : ICommandHandler<CategorizeTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(CategorizeTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new CategorizeTransactionModel(
            command.TransactionId,
            command.NewCategory,
            dateTimeProvider.UtcNow);

        await transactionService.CategorizeAsync(request, cancellationToken);

        await eventBus.PublishAsync(
            new TransactionCategorizedEvent(command.TransactionId, command.NewCategory, request.UpdatedAt),
            cancellationToken);
    }
}