using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Domain.Entities;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;

/// <inheritdoc />
public sealed class TransactionService(
    IRepository<Transaction> transactionRepository,
    ILogger<TransactionService> logger) : ITransactionService
{
    /// <inheritdoc />
    public async Task UpdateAmountAsync(UpdateAmountModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Updating amount for transaction with ID `{TransactionId}` to `{NewAmount}`",
            request.TransactionId, request.NewAmount);

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);
        var amount = Money.Create(request.NewAmount);

        transaction.UpdateAmount(amount);
        await transactionRepository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` updated to new amount `{NewAmount}` successfully.",
            request.TransactionId, request.NewAmount);
    }

    /// <inheritdoc />
    public async Task CategorizeAsync(CategorizeTransactionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Categorizing transaction with ID `{TransactionId}` to category `{NewCategory}`",
            request.TransactionId, nameof(request.NewCategory));

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        transaction.Categorize(request.NewCategory);
        await transactionRepository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` categorized to `{NewCategory}` successfully.",
            request.TransactionId, nameof(request.NewCategory));
    }

    /// <summary>
    /// Retrieves transaction by its ID or throws if not found.
    /// </summary>
    private async Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository
            .Where(t => t.Id == transactionId)
            .SingleOrDefaultAsync(cancellationToken);

        if (transaction is null)
        {
            logger.LogWarning("Transaction with ID `{TransactionId}` not found.", transactionId);
            throw new NotFoundException($"Transaction with ID '{transactionId}' was not found.");
        }

        return transaction;
    }
}