using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;
using IsolationLevel = System.Data.IsolationLevel;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsSynchronizationService(
    IRepository<AccountReadModel, TransactionsReadDbContext> accountsRepository,
    IRepository<TransactionReadModel, TransactionsReadDbContext> transactionsRepository,
    ITransactionManager transactionManager,
    ILogger<AccountsSynchronizationService> logger) : IAccountsSynchronizationService
{
    /// <inheritdoc />
    public async Task SynchronizeAccountCreationAsync(AccountCreationSynchronizationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing account creation with ID `{AccountId}` and name `{Name}`.",
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

        logger.LogInformation("Account with ID `{AccountId}` synchronized successfully.", request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeTransactionAdditionAsync(TransactionAdditionSynchronizationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing addition of transaction with ID `{TransactionId}` to account `{AccountId}`.",
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
            CreatedAt = request.CreatedAt
        };

        account.Balance += request.Amount;

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.AddAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction addition with ID `{TransactionId}` synchronized successfully for account `{AccountId}`.",
            request.TransactionId, request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeTransactionRemovalAsync(TransactionRemovalModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing removal of transaction with ID `{TransactionId}` from account `{AccountId}`.",
            request.TransactionId, request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);

        var transaction = await transactionsRepository
            .Where(t => t.Id == request.TransactionId)
            .SingleOrDefaultAsync(cancellationToken);

        if (transaction is null)
        {
            logger.LogWarning("Transaction with ID `{AccountId}` was not found in synchronization context.", request.AccountId);
            throw new ReadableException($"Transaction with ID '{request.AccountId}' was not found in synchronization context.");
        }

        account.Balance -= transaction.Amount;

        await transactionManager.StartEffect(async ct =>
        {
            await accountsRepository.UpdateAsync(account, ct);
            await transactionsRepository.DeleteAsync(transaction, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction removal with ID `{TransactionId}` synchronized successfully for account `{AccountId}`.",
            request.TransactionId, request.AccountId);
    }

    /// <inheritdoc />
    public async Task SynchronizeAccountDeletionAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Synchronizing deletion of account with ID `{AccountId}`.", request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);
        await accountsRepository.DeleteAsync(account, cancellationToken);

        logger.LogInformation("Account with ID `{AccountId}` synchronized successfully for deletion.", request.AccountId);
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