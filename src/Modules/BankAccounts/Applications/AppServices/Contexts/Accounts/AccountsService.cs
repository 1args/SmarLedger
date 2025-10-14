using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Domain.Entities;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write;
using IdOnlyModel = SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts.IdOnlyModel;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsService(
    IRepository<Account, BackAccountsWriteDbContext> accountsRepository,
    IRepository<Transaction, BackAccountsWriteDbContext> transactionsRepository,
    Lazy<IAuthorizationData> authorizationData,
    ITransactionManager transactionManager,
    ILogger<AccountsService> logger): IAccountsService
{
    /// <inheritdoc />
    public async Task<(Guid AccountId, Guid UserId)> CreateAsync(AccountCreationModel request, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation(
            "Creating account with name {Name} for user with ID {UserId}", 
            request.Name, userId);

        var account = Account.Create(Guid.NewGuid(), request.Name, userId, request.CreatedAt);

        await accountsRepository.AddAsync(account, cancellationToken);

        logger.LogInformation(
            "Account with ID {AccountId} created successfully for user with ID {UserId}",
            account.Id, userId);

        return (account.Id.Value, userId);
    }

    /// <inheritdoc />
    public async Task<(Guid TransactionId, Guid UserId)> AddTransactionAsync(TransactionAdditionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Adding transaction of type {Type} with amount {Amount} to account with ID {AccountId}",
            request.Type, request.Amount, request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);

        var transaction = Transaction.Create(
            Guid.NewGuid(),
            account.Id.Value,
            request.Amount,
            request.Type,
            request.Category,
            request.CreatedAt,
            request.Notes);

        account.ApplyTransaction(transaction);

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.AddAsync(transaction, ct);
            await accountsRepository.UpdateAsync(account, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction added successfully to account with ID {AccountId} with transaction ID {TransactionId}",
            account.Id.Value, transaction.Id.Value);

        return (transaction.Id.Value, account.UserId.Value);
    }

    /// <inheritdoc />
    public async Task RemoveTransactionAsync(TransactionRemovalModel request, CancellationToken cancellationToken)
    {
        var transaction = await GetTransactionAsync(request.TransactionId, cancellationToken);

        logger.LogInformation(
            "Removing transaction with ID {TransactionId} from account with ID {AccountId}",
            transaction.Id.Value, transaction.AccountId.Value);

        var aid = AccountId.Create(request.AccountId);

        var account = await accountsRepository
            .Where(a => a.Id == aid)
            .Include(a => a.Transactions)
            .SingleOrDefaultAsync(cancellationToken) 
            ?? throw new NotFoundException($"Account with ID '{transaction.AccountId.Value}' was not found"); ;

        account.RevertTransaction(transaction);

        await transactionManager.StartEffect(async ct =>
        {
            await transactionsRepository.DeleteAsync(transaction, cancellationToken);
            await accountsRepository.UpdateAsync(account, cancellationToken);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Transaction with ID {TransactionId} removed successfully from account with ID {AccountId}",
            transaction.Id.Value,
            account.Id.Value);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting account with ID {AccountId}", request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);
        await accountsRepository.DeleteAsync(account, cancellationToken);

        logger.LogInformation("Account with ID {AccountId} deleted successfully", request.AccountId);
    }

    /// <summary>
    /// Retrieves an account by its ID or throws if not found.
    /// </summary>
    private async Task<Account> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var aid = AccountId.Create(accountId);

        return await accountsRepository
            .Where(a => a.Id == aid)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Account with ID '{accountId}' was not found.");
    }

    /// <summary>
    /// Retrieves transaction by its ID or throws if not found.
    /// </summary>
    private async Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        var tid = TransactionId.Create(transactionId);

        return await transactionsRepository
            .Where(t => t.Id == tid)
            .SingleOrDefaultAsync(cancellationToken) 
            ?? throw new NotFoundException($"Transaction with ID '{transactionId}' was not found.");
    }
}