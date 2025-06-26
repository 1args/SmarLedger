using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateTransactionAmount;

/// <summary>
/// Handles the logic for processing <see cref="UpdateTransactionAmountCommand"/>.
/// </summary>
public sealed class UpdateTransactionAmountCommandHandler(
    ITransactionService transactionService) : ICommandHandler<UpdateTransactionAmountCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(UpdateTransactionAmountCommand command, CancellationToken cancellationToken)
    {
        var request = new UpdateAmountModel(
            command.TransactionId,
            command.NewAmount);

        await transactionService.UpdateAmountAsync(request, cancellationToken);
    }
}