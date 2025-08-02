using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsSynchronizationService(
    IRepository<AccountReadModel, TransactionsReadDbContext> accountsRepository,
    IRepository<TransactionReadModel, TransactionsReadDbContext> transactionsRepository,
    ITransactionManager transactionManager,
    HybridCache cache,
    ILogger<AccountsSynchronizationService> logger) : IAccountsSynchronizationService
{
    /// <inheritdoc />
    public async Task SynchronizeAccountCreationAsync(AccountCreationSynchronizationModel request, CancellationToken cancellationToken)
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
        await cache.RemoveAsync($"accounts:user:{request.UserId}:*", cancellationToken);

        logger.LogInformation(
            "Account with ID {AccountId} was successfully synchronized after creation",
            request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeTransactionAdditionAsync(TransactionAdditionSynchronizationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing addition of transaction with ID {TransactionId} to account {AccountId}",
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

        account.Balance = transaction.Type switch
        {
            "Income" => account.Balance + transaction.Amount,
            "Expense" => account.Balance - transaction.Amount,
            _ => account.Balance
        };

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.AddAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        await cache.RemoveAsync($"account:{request.AccountId}", cancellationToken);
        await cache.RemoveAsync($"accounts:user:{account.UserId}:*", cancellationToken);
        await cache.RemoveAsync($"transactions:account:{request.AccountId}:*", cancellationToken);

        logger.LogInformation(
            "Transaction with ID {TransactionId} was successfully synchronized after addition to account {AccountId}",
            request.TransactionId, request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeTransactionRemovalAsync(TransactionRemovalSynchronizationModel request, CancellationToken cancellationToken)
    {
        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        logger.LogInformation(
            "Synchronizing removal of transaction with ID {TransactionId} from account `{AccountId}",
            transaction.Id, transaction.AccountId);

        var account = await GetAccountAsync(transaction.AccountId, cancellationToken);

        account.Balance = transaction.Type switch
        {
            "Income" => account.Balance - transaction.Amount,
            "Expense" => account.Balance + transaction.Amount,
            _ => account.Balance
        };

        account.LastUpdatedAt = request.AccountUpdatedAt;

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.DeleteAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        await cache.RemoveAsync($"account:{transaction.AccountId}", cancellationToken);
        await cache.RemoveAsync($"accounts:user:{account.UserId}:*", cancellationToken);
        await cache.RemoveAsync($"transactions:account:{transaction.AccountId}:*", cancellationToken);

        logger.LogInformation(
            "Transaction with ID {TransactionId} was successfully synchronized after removal from account {AccountId}",
            request.TransactionId, account.Id);
    }

    /// <inheritdoc />
    public async Task SynchronizeAccountDeletionAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Synchronizing deletion of account with ID {AccountId}", request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);
        await accountsRepository.DeleteAsync(account, cancellationToken);

        await cache.RemoveAsync($"account:{request.AccountId}", cancellationToken);
        await cache.RemoveAsync($"accounts:user:{account.UserId}:*", cancellationToken);
        await cache.RemoveAsync($"transactions:account:{request.AccountId}:*", cancellationToken);

        logger.LogInformation(
            "Account with ID {AccountId} was successfully synchronized after deletion",
            request.AccountId);
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
            logger.LogWarning("Account with ID {AccountId} not found in synchronization context", accountId);
            throw new NotFoundException($"Account with ID '{accountId}' was not found in synchronization context.");
        }

        return account;
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
            logger.LogWarning("Transaction with ID {TransactionId} not found in synchronization context", transactionId);
            throw new ReadableException($"Transaction with ID '{transactionId}' was not found in synchronization context.");
        }

        return transaction;
    }
}