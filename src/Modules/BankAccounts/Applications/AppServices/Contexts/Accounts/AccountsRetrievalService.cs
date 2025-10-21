using Microsoft.EntityFrameworkCore;
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
using SmartLedger.Modules.BankAccounts.Contracts.Mappers;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Transactions;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountsRetrievalService(
    IRepository<AccountReadModel, BackAccountsReadDbContext> accountsRepository,
    IRepository<TransactionReadModel, BackAccountsReadDbContext> transactionsRepository,
    Lazy<IAuthorizationData> authorizationData,
    ILogger<AccountsRetrievalService> logger) : IAccountsRetrievalService
{
    /// <inheritdoc />
    public async Task<AccountResponse> GetAccountAsync(
        Guid accountId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving account with ID {AccountId}", accountId);

        var account = await accountsRepository
            .Where(a => a.Id == accountId)
            .Select(a => a.MapToResponse())
            .SingleOrDefaultAsync(cancellationToken) 
            ?? throw new NotFoundException($"Account with ID '{accountId}' was not found."); ;
 
        logger.LogInformation("Account with ID {AccountId} retrieved successfully", accountId);

        return account;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<AccountListItem>> GetAccountsPageAsync(
        GetAccountsPageModel filter,
        CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Retrieving paginated accounts for user with ID {UserId}", userId);

        var combinedSpecification = new AccountByUserIdSpecification(userId)
            .And(new AccountByBalanceRangeSpecification(filter.MinBalance, filter.MaxBalance))
            .And(new AccountByDateRangeSpecification(filter.StartDate, filter.EndDate));

        var accounts = accountsRepository
            .Where(combinedSpecification.Criteria)
            .Select(a => a.MapToListItem());

        var accountsPage = await PaginatedList<AccountListItem>.CreateAsync(accounts, filter, cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} accounts (Page {PageNumber} of {TotalPages}) for user with ID {UserId}",
            accountsPage.Items.Count,
            accountsPage.PageNumber,
            accountsPage.TotalPages,
            userId);

        return accountsPage;
    }

    /// <inheritdoc />
    public async Task<TransactionResponse> GetTransactionAsync(
        GetTransactionModel request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving transaction with ID {TransactionId} from account with ID {AccountId}", 
            request.TransactionId, request.AccountId);

        var transaction = await transactionsRepository
            .Where(t => t.Id == request.TransactionId)
            .Select(t => t.MapToResponse())
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Transaction with ID '{request.TransactionId}' was not found."); ;

        logger.LogInformation("Transaction with ID {TransactionId} retrieved successfully", request.TransactionId);

        return transaction;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<TransactionListItem>> GetTransactionsPageAsync(
        GetTransactionsPageModel filter,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated transactions for account with ID {AccountId}", filter.AccountId);

        var combinedSpecification = new TransactionByAccountIdSpecification(filter.AccountId)
            .And(new TransactionByAmountRangeSpecification(filter.MinAmount, filter.MaxAmount))
            .And(new TransactionByTypeSpecification(filter.Type))
            .And(new TransactionByCategorySpecification(filter.Category))
            .And(new TransactionByDateRangeSpecification(filter.StartDate, filter.EndDate));

        var transactions = transactionsRepository
            .Where(combinedSpecification.Criteria)
            .Select(t => t.MapToListItem())
            .OrderBy(t => t.CreatedAt);

        var transactionsPage = await PaginatedList<TransactionListItem>.CreateAsync(
            transactions,
            filter, 
            cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} transactions (Page {PageNumber} of {TotalPages}) for account with ID {AccountId}",
            transactionsPage.Items.Count,
            transactionsPage.PageNumber,
            transactionsPage.TotalPages,
            filter.AccountId);

        return transactionsPage;
    }
}