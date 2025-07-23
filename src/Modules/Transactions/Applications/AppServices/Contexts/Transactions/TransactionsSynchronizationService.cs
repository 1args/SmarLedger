using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;

/// <inheritdoc />
public sealed class TransactionsSynchronizationService(
    IRepository<TransactionReadModel, TransactionsReadDbContext> transactionsRepository,
    IRepository<AccountReadModel, TransactionsReadDbContext> accountsRepository,
    ITransactionManager transactionManager,
    HybridCache cache,
    ILogger<TransactionsSynchronizationService> logger) : ITransactionsSynchronizationService
{
    /// <inheritdoc />
    public async Task SynchronizeAmountChangeAsync(UpdateAmountModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing transaction amount for transaction with ID `{TransactionId}` to `{NewAmount}`...",
            request.TransactionId, request.NewAmount);

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);
        var account = await GetAccountAsync(transaction.AccountId, cancellationToken);

        var amountDifference = request.NewAmount - transaction.Amount;

        transaction.Amount = request.NewAmount;
        transaction.LastUpdatedAt = request.UpdatedAt;

        account.Balance = transaction.Type switch
        {
            "Income" => account.Balance + amountDifference,
            "Expense" => account.Balance - amountDifference,
            _ => account.Balance
        };
        account.LastUpdatedAt = request.UpdatedAt;

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.UpdateAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        await cache.RemoveAsync($"transaction:{request.TransactionId}", cancellationToken);
        await cache.RemoveAsync($"transactions:account:{transaction.AccountId}:*", cancellationToken);
        await cache.RemoveAsync($"account:{transaction.AccountId}", cancellationToken);
        await cache.RemoveAsync($"accounts:user:{account.UserId}:*", cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` was successfully synchronized after amount change.",
            request.TransactionId);
    }

    /// <inheritdoc />
    public async Task SynchronizeCategoryChangeAsync(CategorizeTransactionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing transaction category for transaction with ID `{TransactionId}` to `{NewCategory}`...",
            request.TransactionId, request.NewCategory);

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        transaction.Category = request.NewCategory.ToString();
        transaction.LastUpdatedAt = request.UpdatedAt;

        await transactionsRepository.UpdateAsync(transaction, cancellationToken);

        await cache.RemoveAsync($"transaction:{request.TransactionId}", cancellationToken);
        await cache.RemoveAsync($"transactions:account:{transaction.AccountId}:*", cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` was successfully synchronized after category change.",
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
            throw new ReadableException($"Transaction with ID '{transactionId}' was not found in synchronization context.");
        }

        return transaction;
    }

    /// <summary>
    /// Retrieves an account by its ID or throws if not found.
    /// </summary>
    private async Task<AccountReadModel> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await accountsRepository
            .Where(a => a.Id == accountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            logger.LogWarning("Account with ID `{AccountId}` not found in synchronization context.", accountId);
            throw new NotFoundException($"Account with ID '{accountId}' was not found in synchronization context.");
        }

        return account;
    }
}