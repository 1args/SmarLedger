using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateAmount;

/// <summary>
/// Represents a command to update the amount of a transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewAmount">New amount.</param>
public sealed record UpdateAmountCommand(
    Guid TransactionId,
    decimal NewAmount) : ICommand;