using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;

/// <summary>
/// Provides functionality for managing transactions.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Updates the amount of an existing transaction.
    /// </summary>
    /// <param name="request">Model containing transaction ID and new amount.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdateAmountAsync(UpdateAmountModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Changes the category of an existing transaction.
    /// </summary>
    /// <param name="request">Model containing transaction ID and new category ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task CategorizeAsync(CategorizeTransactionModel request, CancellationToken cancellationToken);
}