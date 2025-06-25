using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.Entities;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountService(
    IRepository<Account> accountRepository,
    IRepository<Transaction> transactionRepository,
    ILogger<AccountService> logger): IAccountService
{
    /// <inheritdoc />
    public async Task CreateAsync(CreateAccountModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating account with name `{Name}` for user `{UserId}`.", 
            request.Name, request.UserId);

        var name = AccountName.Create(request.Name);
        var account = Account.Create(name, request.UserId, request.CreatedAt);

        await accountRepository.AddAsync(account, cancellationToken);

        logger.LogInformation("Account with ID `{AccountId}` created successfully.", account.Id);
    }

    /// <inheritdoc />
    public async Task AddTransactionAsync(AddTransactionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Adding transaction of type `{Type}` with amount `{Amount}` to account `{AccountId}`.",
            request.Type, request.Amount, request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);

        var transaction = Transaction.Create(
            account.Id,
            Money.Create(request.Amount),
            request.Type,
            request.Category,
            request.CreateAt,
            TransactionDescription.Create(request.Notes));

        account.ApplyTransaction(transaction);

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await accountRepository.UpdateAsync(account, cancellationToken);

        logger.LogInformation(
            "Transaction added successfully to account `{AccountId}` with transaction ID `{TransactionId}`.",
            request.AccountId, transaction.Id);
    }

    /// <inheritdoc />
    public async Task RemoveTransactionAsync(RemoveTransactionModel request, CancellationToken cancellationToken)
    {
        var account = await accountRepository
            .Where(a => a.Id == request.AccountId)
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            logger.LogWarning("Account with ID `{AccountId}` not found.", request.AccountId);
            throw new NotFoundException($"Account with ID '{request.AccountId}' was not found");
        }

        var transaction = await transactionRepository
            .Where(t => t.Id == request.TransactionId)
            .SingleOrDefaultAsync(cancellationToken);

        if (transaction is null)
        {
            logger.LogWarning("Transaction with ID `{TransactionId}` not found.", request.TransactionId);
            throw new NotFoundException($"Account with ID '{request.TransactionId}' was not found");
        }

        account.RevertTransaction(transaction);

        await transactionRepository.DeleteAsync([transaction], cancellationToken);
        await accountRepository.UpdateAsync(account, cancellationToken);

        logger.LogInformation(
            "Transaction with ID `{TransactionId}` removed successfully from account `{AccountId}`.",
            request.TransactionId,
            request.AccountId);

    }

    /// <inheritdoc />
    public async Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting account with ID `{AccountId}`.", request.AccountId);

        var account = await GetAccountAsync(request.AccountId, cancellationToken);
        await accountRepository.DeleteAsync([account], cancellationToken);

        logger.LogInformation("Account with ID `{AccountId}` deleted successfully.", request.AccountId);
    }

    /// <summary>
    /// Retrieves an account by its ID or throws if not found.
    /// </summary>
    private async Task<Account> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await accountRepository
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