using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Specifications;
using SmartLedger.Modules.Transactions.Contracts.Mappers;
using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsRetrievalService(
    IRepository<AccountReadModel, TransactionsReadDbContext> accountsRepository,
    ILogger<AccountsRetrievalService> logger) : IAccountsRetrievalService
{
    /// <inheritdoc />
    public async Task<AccountResponse> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving account with ID `{AccountId}`...", accountId);

        var account = await accountsRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(a => a.Id == accountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            logger.LogWarning("Account with ID `{AccountId}` not found.", accountId);
            throw new NotFoundException($"Account with ID '{accountId}' was not found.");
        }

        logger.LogInformation("Account with ID `{AccountId}` retrieved successfully.", accountId);

        return account.MapToResponse();
    }

    /// <inheritdoc />
    public async Task<PaginatedList<AccountListItem>> GetPaginatedAccountsAsync(
        GetPaginatedAccountsModel filter, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated accounts for user with ID `{UserId}`...", filter.UserId);
        
        var combinedSpecification = new AccountByUserIdSpecification(filter.UserId)
           .And(new AccountByBalanceRangeSpecification(filter.MinBalance, filter.MaxBalance))
           .And(new AccountByDateRangeSpecification(filter.StartDate, filter.EndDate));

        var accounts = accountsRepository
           .AsQueryable()
           .AsNoTracking()
           .Where(combinedSpecification)
           .Select(a => a.MapToListItem());

        var paginatedAccounts = await PaginatedList<AccountListItem>
            .CreateAsync(accounts, filter, cancellationToken);

        logger.LogInformation(
            "Successfully retrieved `{Count}` accounts (Page `{PageNumber}` of `{TotalPages}`) for user with ID `{UserId}`.",
            paginatedAccounts.Items.Count,
            paginatedAccounts.PageNumber,
            paginatedAccounts.TotalPages,
            filter.UserId);

        return paginatedAccounts;
    }
}