using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateAmount;

/// <summary>
/// Handles the logic for processing <see cref="UpdateAmountCommand"/>.
/// </summary>
public sealed class UpdateAmountCommandHandler(
    ITransactionService transactionService) : ICommandHandler<UpdateAmountCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(UpdateAmountCommand command, CancellationToken cancellationToken)
    {
        var request = new UpdateAmountModel(
            command.TransactionId,
            command.NewAmount);

        await transactionService.UpdateAmountAsync(request, cancellationToken);
    }
}