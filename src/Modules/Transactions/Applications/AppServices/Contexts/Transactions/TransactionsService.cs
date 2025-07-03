using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.Entities;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;

/// <inheritdoc />
public sealed class TransactionsService(
    IRepository<Transaction, TransactionsWriteDbContext> transactionsRepository,
    IRepository<Account, TransactionsWriteDbContext> accountsRepository,
    ITransactionManager transactionManager,
    ILogger<TransactionsService> logger) : ITransactionsService
{
    /// <inheritdoc />
    public async Task UpdateAmountAsync(UpdateAmountModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Updating amount for transaction with ID `{TransactionId}` to `{NewAmount}`.",
            request.TransactionId, request.NewAmount);

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);
        var account = await GetAccountAsync(transaction.AccountId, cancellationToken);

        var oldMoney = transaction.Amount;
        var newMoney = Money.Create(request.NewAmount);

        account.RevertTransaction(transaction);
        transaction.UpdateAmount(newMoney);
        account.ApplyTransaction(transaction);

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.UpdateAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` updated to new amount `{NewAmount}` successfully.",
            request.TransactionId, request.NewAmount);
    }

    /// <inheritdoc />
    public async Task CategorizeAsync(CategorizeTransactionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Categorizing transaction with ID `{TransactionId}` to category `{NewCategory}`.",
            request.TransactionId, nameof(request.NewCategory));

        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        transaction.Categorize(request.NewCategory);
        await transactionsRepository.UpdateAsync(transaction, cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` categorized to `{NewCategory}` successfully.",
            request.TransactionId, nameof(request.NewCategory));
    }

    /// <summary>
    /// Retrieves transaction by its ID or throws if not found.
    /// </summary>
    private async Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = await transactionsRepository
            .Where(t => t.Id == transactionId)
            .SingleOrDefaultAsync(cancellationToken);

        if (transaction is null)
        {
            logger.LogWarning("Transaction with ID `{TransactionId}` not found.", transactionId);
            throw new NotFoundException($"Transaction with ID '{transactionId}' was not found.");
        }

        return transaction;
    }

    /// <summary>
    /// Retrieves an account by its ID or throws if not found.
    /// </summary>
    private async Task<Account> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await accountsRepository
            .Where(a => a.Id == accountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            logger.LogWarning("Account with ID `{AccountId}` not found.", accountId);
            throw new NotFoundException($"Account with ID '{accountId}' was not found.");
        }

        return account;
    }
}