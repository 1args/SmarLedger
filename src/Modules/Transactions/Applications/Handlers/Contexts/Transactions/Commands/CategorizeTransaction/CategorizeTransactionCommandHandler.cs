using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.CategorizeTransaction;

/// <summary>
/// Handles the logic for processing <see cref="CategorizeTransactionCommand"/>.
/// </summary>
public sealed class CategorizeTransactionCommandHandler(
    ITransactionService transactionService) : ICommandHandler<CategorizeTransactionCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(CategorizeTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = new CategorizeTransactionModel(
            command.TransactionId,
            command.NewCategory);

        await transactionService.CategorizeAsync(request, cancellationToken);
    }
}