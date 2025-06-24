using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

/// <inheritdoc />
public sealed class AccountService(
    IRepository<Account> accountRepository,
    ILogger<AccountService> logger): IAccountService
{
    /// <inheritdoc />
    public async Task CreateAsync(CreateAccountModel request, CancellationToken cancellationToken)
    {
        var name = AccountName.Create(request.Name);

        var account = Account.Create(name, request.UserId);
        await accountRepository.AddAsync(account, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddTransactionAsync(AddRemoveTransactionModel request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task RemoveTransactionAsync(AddRemoveTransactionModel request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        var account = await GetAccountAsync(request.AccountId, cancellationToken);

        await accountRepository.DeleteAsync([account], cancellationToken);
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
            throw new NotFoundException($"Account with ID '{accountId}' was not found.");
        }

        return account;
    }
}