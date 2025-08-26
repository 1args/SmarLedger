using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Specifications.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Specifications.Transactions;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;
using SmartLedger.Modules.Transactions.Contracts.Mappers;
using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsRetrievalService(
    IRepository<AccountReadModel, BackAccountsReadDbContext> accountsRepository,
    IRepository<TransactionReadModel, BackAccountsReadDbContext> transactionsRepository,
    Lazy<IAuthorizationData> authorizationData,
    IHybridCache cache,
    ILogger<AccountsRetrievalService> logger) : IAccountsRetrievalService
{
    /// <inheritdoc />
    public async Task<AccountResponse> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving account with ID {AccountId}", accountId);

        var cacheKey = $"account:{accountId}";
        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(1),
            LocalCacheExpiration = TimeSpan.FromSeconds(15)
        };

        var account = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
                var result = await accountsRepository
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(a => a.Id == accountId)
                    .SingleOrDefaultAsync(ct) 
                    ?? throw new NotFoundException($"Account with ID '{accountId}' was not found.");

                return result.MapToResponse();
            }, cancellationToken: cancellationToken);

        logger.LogInformation("Account with ID {AccountId} retrieved successfully", accountId);
        return account;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<AccountListItem>> GetPaginatedAccountsAsync(
        GetPaginatedAccountsModel filter,
        CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Retrieving paginated accounts for user with ID {UserId}", userId);

        var cacheKey = $"accounts:user:{userId}:page:{filter.PageNumber}:minbalance:{filter.MinBalance}" +
                       $":maxbalance:{filter.MaxBalance}:start:{filter.StartDate:yyyy-MM-dd}:end:{filter.EndDate:yyyy-MM-dd}";
        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromSeconds(15),
            LocalCacheExpiration = TimeSpan.FromSeconds(10)
        };

        var paginatedAccounts = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
                var combinedSpecification = new AccountByUserIdSpecification(userId)
                    .And(new AccountByBalanceRangeSpecification(filter.MinBalance, filter.MaxBalance))
                    .And(new AccountByDateRangeSpecification(filter.StartDate, filter.EndDate));

                var accounts = accountsRepository
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(combinedSpecification)
                    .Select(a => a.MapToListItem());

                return await PaginatedList<AccountListItem>.CreateAsync(accounts, filter, ct);
            }, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} accounts (Page {PageNumber} of {TotalPages}) for user with ID {UserId}",
            paginatedAccounts.Items.Count,
            paginatedAccounts.PageNumber,
            paginatedAccounts.TotalPages,
            userId);

        return paginatedAccounts;
    }

    /// <inheritdoc />
    public async Task<TransactionResponse> GetTransactionAsync(GetTransactionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving transaction with ID {TransactionId} from account with ID {AccountId}", 
            request.TransactionId, request.AccountId);

        var cacheKey = $"transaction:{request.TransactionId}";
        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(5),
            LocalCacheExpiration = TimeSpan.FromSeconds(30)
        };

        var transaction = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
                var result = await transactionsRepository
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(t => t.Id == request.TransactionId)
                    .SingleOrDefaultAsync(ct)
                    ?? throw new NotFoundException($"Transaction with ID '{request.TransactionId}' was not found.");

                return result.MapToResponse();
            }, cancellationToken: cancellationToken);

        logger.LogInformation("Transaction with ID {TransactionId} retrieved successfully", request.TransactionId);
        return transaction;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<TransactionListItem>> GetPaginatedTransactionsAsync(
        GetPaginatedTransactionsModel filter,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated transactions for account with ID {AccountId}", filter.AccountId);

        var cacheKey = $"transactions:account:{filter.AccountId}:page:{filter.PageNumber}:type:{filter.Type ?? "none"}" +
                       $":category:{filter.Category ?? "none"}:minamount:{filter.MinAmount}:maxamount:{filter.MaxAmount}" +
                       $":start:{filter.StartDate:yyyy-MM-dd}:end:{filter.EndDate:yyyy-MM-dd}";
        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromSeconds(15),
            LocalCacheExpiration = TimeSpan.FromSeconds(10)
        };

        var paginatedTransactions = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
                var combinedSpecification = new TransactionByAccountIdSpecification(filter.AccountId)
                    .And(new TransactionByAmountRangeSpecification(filter.MinAmount, filter.MaxAmount))
                    .And(new TransactionByTypeSpecification(filter.Type))
                    .And(new TransactionByCategorySpecification(filter.Category))
                    .And(new TransactionByDateRangeSpecification(filter.StartDate, filter.EndDate));

                var query = transactionsRepository
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(combinedSpecification)
                    .OrderBy(t => t.CreatedAt)
                    .Select(t => t.MapToListItem());

                return await PaginatedList<TransactionListItem>.CreateAsync(query, filter, ct);
            }, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} transactions (Page {PageNumber} of {TotalPages}) for account with ID {AccountId}",
            paginatedTransactions.Items.Count,
            paginatedTransactions.PageNumber,
            paginatedTransactions.TotalPages,
            filter.AccountId);

        return paginatedTransactions;
    }
}