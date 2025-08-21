using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Specifications;
using SmartLedger.Modules.Transactions.Contracts.Mappers;
using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsRetrievalService(
    IRepository<AccountReadModel, TransactionsReadDbContext> accountsRepository,
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
                    .SingleOrDefaultAsync(ct);

                if (result is null)
                {
                    logger.LogWarning("Account with ID {AccountId} not found", accountId);
                    throw new NotFoundException($"Account with ID '{accountId}' was not found.");
                }

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
}