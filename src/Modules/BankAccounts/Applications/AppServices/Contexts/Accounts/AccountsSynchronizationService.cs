using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;
using IdOnlyModel = SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts.IdOnlyModel;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsSynchronizationService(
    IRepository<AccountReadModel, BackAccountsReadDbContext> accountsRepository,
    IRepository<TransactionReadModel, BackAccountsReadDbContext> transactionsRepository,
    ITransactionManager transactionManager,
    ILogger<AccountsSynchronizationService> logger) : IAccountsSynchronizationService
{
    /// <inheritdoc />
    public async Task SynchronizeAccountCreationAsync(
        AccountCreationSynchronizationModel request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing account creation with ID {AccountId} and name {Name}",
            request.AccountId, request.Name);

        var account = new AccountReadModel
        {
            Id = request.AccountId,
            UserId = request.UserId,
            Name = request.Name,
            Balance = 0.0m,
            CreatedAt = request.CreatedAt,
            LastUpdatedAt = request.CreatedAt
        };

        await accountsRepository.AddAsync(account, cancellationToken);

        logger.LogInformation(
            "Account with ID {AccountId} was successfully synchronized after creation",
            request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeAccountDeletionAsync(
        IdOnlyModel request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Synchronizing deletion of account with ID {AccountId}", request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);
        await accountsRepository.DeleteAsync(account, cancellationToken);

        logger.LogInformation(
            "Account with ID {AccountId} was successfully synchronized after deletion",
            request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeTransactionCreationAsync(
        TransactionCreationSynchronizationModel request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing addition of transaction with ID {TransactionId} to account with ID {AccountId}",
            request.TransactionId, request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);

        var transaction = new TransactionReadModel
        {
            Id = request.TransactionId,
            AccountId = request.AccountId,
            UserId = account.UserId,
            Amount = request.Amount,
            Type = request.Type.ToString(),
            Category = request.Category.ToString(),
            Notes = request.Notes,
            AccountName = account.Name,
            CreatedAt = request.CreatedAt,
            LastUpdatedAt = request.CreatedAt
        };

        account.Balance = ApplyTransactionToBalance(
            account.Balance,
            transaction.Type,
            transaction.Amount);

        await transactionManager.StartEffectAsync(async ct =>
        {
            await transactionsRepository.AddAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction with ID {TransactionId} was successfully synchronized after addition to account with ID {AccountId}",
            request.TransactionId, request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeTransactionDeletionAsync(
        TransactionDeletionSynchronizationModel request, 
        CancellationToken cancellationToken)
    {
        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        logger.LogInformation(
            "Synchronizing removal of transaction with ID {TransactionId} from account with ID {AccountId}",
            transaction.Id, transaction.AccountId);

        var account = await GetAccountAsync(transaction.AccountId, cancellationToken);

        account.Balance = RevertTransactionFromBalance(
            account.Balance,
            transaction.Type,
            transaction.Amount);

        account.LastUpdatedAt = request.AccountUpdatedAt;

        await transactionManager.StartEffectAsync(async ct =>
        {
            await transactionsRepository.DeleteAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction with ID {TransactionId} was successfully synchronized after removal from account with ID {AccountId}",
            request.TransactionId, account.Id);
    }

    /// <summary>
    /// Applies a balance change after creating a transaction.
    /// </summary>
    private static decimal ApplyTransactionToBalance(
        decimal currentBalance,
        string transactionType, 
        decimal amount)
    {
        return transactionType switch
        {
            "Income" => currentBalance + amount,
            "Expense" => currentBalance - amount,
            _ => currentBalance
        };
    }

    /// <summary>
    /// Cancels the balance change after deleting the transaction.
    /// </summary>
    private static decimal RevertTransactionFromBalance(
       decimal currentBalance,
       string transactionType,
       decimal amount)
    {
        return transactionType switch
        {
            "Income" => currentBalance - amount,
            "Expense" => currentBalance + amount,
            _ => currentBalance
        };
    }

    /// <summary>
    /// Retrieves an account by its ID or throws if not found.
    /// </summary>
    private async Task<AccountReadModel> GetAccountAsync(
        Guid accountId, 
        CancellationToken cancellationToken)
    {
        var account = await accountsRepository
            .Where(a => a.Id == accountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            logger.LogWarning("Account with ID {AccountId} not found in synchronization context", accountId);
            throw new NotFoundException($"Account with ID '{accountId}' was not found in synchronization context.");
        }

        return account;
    }

    /// <summary>
    /// Retrieves transaction by its ID or throws if not found.
    /// </summary>
    private async Task<TransactionReadModel> GetTransactionAsync(
        Guid transactionId, 
        CancellationToken cancellationToken)
    {
        var transaction = await transactionsRepository
            .Where(t => t.Id == transactionId)
            .SingleOrDefaultAsync(cancellationToken);

        if (transaction is null)
        {
            logger.LogWarning("Transaction with ID {TransactionId} not found in synchronization context", transactionId);
            throw new ReadableException($"Transaction with ID '{transactionId}' was not found in synchronization context.");
        }

        return transaction;
    }
}