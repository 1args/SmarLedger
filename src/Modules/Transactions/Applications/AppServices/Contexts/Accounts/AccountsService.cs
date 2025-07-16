using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.Entities;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsService(
    IRepository<Account, TransactionsWriteDbContext> accountsRepository,
    IRepository<Transaction, TransactionsWriteDbContext> transactionsRepository,
    ITransactionManager transactionManager,
    ILogger<AccountsService> logger): IAccountsService
{
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(AccountCreationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating account with name `{Name}` for user with ID `{UserId}`...", 
            request.Name, request.UserId);

        var name = AccountName.Create(request.Name);
        var account = Account.Create(name, request.UserId, request.CreatedAt);

        await accountsRepository.AddAsync(account, cancellationToken);

        logger.LogInformation(
            "Account with ID `{AccountId}` created successfully for user with ID `{UserId}`.",
            account.Id, account.UserId);

        return account.Id;
    }

    /// <inheritdoc />
    public async Task<(Guid TransactionId, Guid UserId)> AddTransactionAsync(TransactionAdditionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Adding transaction of type `{Type}` with amount `{Amount}` to account `{AccountId}`...",
            request.Type, request.Amount, request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);

        var transaction = Transaction.Create(
            account.Id,
            Money.Create(request.Amount),
            request.Type,
            request.Category,
            request.CreatedAt,
            TransactionDescription.Create(request.Notes));

        account.ApplyTransaction(transaction);

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.AddAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction added successfully to account `{AccountId}` with transaction ID `{TransactionId}`.",
            account.Id, transaction.Id);

        return (transaction.Id, account.UserId);
    }

    /// <inheritdoc />
    public async Task RemoveTransactionAsync(TransactionRemovalModel request, CancellationToken cancellationToken)
    {
        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        logger.LogInformation(
            "Removing transaction with ID `{TransactionId}` from account with ID `{AccountId}`...",
            transaction.Id, transaction.AccountId);

        var account = await accountsRepository
            .Where(a => a.Id == transaction.AccountId)
            .Include(a => a.Transactions)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            logger.LogWarning("Account with ID `{AccountId}` not found.", transaction.AccountId);
            throw new NotFoundException($"Account with ID '{transaction.AccountId}' was not found");
        }

        account.RevertTransaction(transaction);

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.DeleteAsync(transaction, cancellationToken);
            await accountsRepository.UpdateAsync(account, cancellationToken);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` removed successfully from account `{AccountId}`.",
            transaction.Id,
            account.Id);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting account with ID `{AccountId}`...", request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);
        await accountsRepository.DeleteAsync(account, cancellationToken);

        logger.LogInformation("Account with ID `{AccountId}` deleted successfully.", request.AccountId);
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
}