using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;

/// <summary>
/// Provides functionality for synchronizing changes in transactions between write and read models.
/// </summary>
public interface ITransactionsSynchronizationService
{
    /// <summary>
    /// Synchronizes changes to the transaction amount.
    /// </summary>
    /// <param name="request">Model containing transaction ID and new amount.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeAmountChangeAsync(UpdateAmountModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the change of transaction category.
    /// </summary>
    /// <param name="request">Model containing transaction ID and new category ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeCategoryChangeAsync(CategorizeTransactionModel request, CancellationToken cancellationToken);
}