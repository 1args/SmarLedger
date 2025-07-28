using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;
using SmartLedger.Modules.Transactions.Contracts.Mappers;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions;

/// <inheritdoc />
public sealed class TransactionsRetrievalService(
    IRepository<TransactionReadModel, TransactionsReadDbContext> transactionsRepository,
    HybridCache cache,
    ILogger<ITransactionsRetrievalService> logger) : ITransactionsRetrievalService
{
    /// <inheritdoc />
    public async Task<TransactionResponse> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving transaction with ID `{TransactionId}`...", transactionId);

        var cacheKey = $"transaction:{transactionId}";
        var cacheOptions = new HybridCacheEntryOptions()
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
                    .Where(t => t.Id == transactionId)
                    .SingleOrDefaultAsync(ct);

                if (result is null)
                {
                    logger.LogWarning("Transaction with ID `{TransactionId}` not found.", transactionId);
                    throw new NotFoundException($"Transaction with ID '{transactionId}' was not found.");
                }

                return result.MapToResponse();
            }, cancellationToken: cancellationToken);

        logger.LogInformation("Transaction with ID `{TransactionId}` retrieved successfully.", transactionId);
        return transaction;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<TransactionListItem>> GetPaginatedTransactionsAsync(
        GetPaginatedTransactionsModel filter,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated transactions for account with ID `{AccountId}`...", filter.AccountId);

        var cacheKey = $"transactions:account:{filter.AccountId}:page:{filter.PageNumber}:type:{filter.Type ?? "none"}" +
                       $":category:{filter.Category ?? "none"}:minamount:{filter.MinAmount}:maxamount:{filter.MaxAmount}" +
                       $":start:{filter.StartDate:yyyy-MM-dd}:end:{filter.EndDate:yyyy-MM-dd}";
        var cacheOptions = new HybridCacheEntryOptions()
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
            "Successfully retrieved `{Count}` transactions (Page `{PageNumber}` of `{TotalPages}`) for account with ID `{AccountId}`.",
            paginatedTransactions.Items.Count,
            paginatedTransactions.PageNumber,
            paginatedTransactions.TotalPages,
            filter.AccountId);

        return paginatedTransactions;
    }
}