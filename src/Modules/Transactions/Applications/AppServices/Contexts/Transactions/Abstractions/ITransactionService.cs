using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;

/// <summary>
/// Provides functionality for managing transactions.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Creates a new transaction using the specified model.
    /// </summary>
    /// <param name="request">Model containing data to create the transaction.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task CreateAsync(CreateTransactionModel request, CancellationToken cancellationToken);

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

    /// <summary>
    /// Deletes a transaction by its identifier.
    /// </summary>
    /// <param name="request">Model containing transaction ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken);
}