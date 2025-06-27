using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;

/// <inheritdoc />
public sealed class TransactionsSynchronizationService(
    IRepository<TransactionReadModel> transactionsRepository,
    ILogger<TransactionsSynchronizationService> logger) : ITransactionsSynchronizationService
{
    /// <inheritdoc />
    public async Task SynchronizeAmountChangeAsync(UpdateAmountModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing transaction amount for transaction with ID `{TransactionId}` to `{NewAmount}`.",
            request.TransactionId, request.NewAmount);

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        transaction.Amount = request.NewAmount;
        transaction.LastUpdatedAt = request.UpdatedAt;

        await transactionsRepository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation(
            "Transaction amount for transaction ID `{TransactionId}` synchronized successfully.",
            request.TransactionId);
    }

    /// <inheritdoc />
    public async Task SynchronizeCategoryChangeAsync(CategorizeTransactionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing transaction category for transaction with ID `{TransactionId}` to `{NewCategory}`.",
            request.TransactionId, request.NewCategory);

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        transaction.Category = request.NewCategory.ToString();
        transaction.LastUpdatedAt = request.UpdatedAt;

        await transactionsRepository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation(
            "Transaction category for transaction ID `{TransactionId}` synchronized successfully.",
            request.TransactionId);
    }

    /// <summary>
    /// Retrieves transaction by its ID or throws if not found.
    /// </summary>
    private async Task<TransactionReadModel> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = await transactionsRepository
            .Where(t => t.Id == transactionId)
            .SingleOrDefaultAsync(cancellationToken);

        if (transaction is null)
        {
            logger.LogWarning("Transaction with ID `{TransactionId}` not found in synchronization context.", transactionId);
            throw new ReadableException($"Transaction with ID '{transactionId}' was not found in synchronization context. ");
        }

        return transaction;
    }
}