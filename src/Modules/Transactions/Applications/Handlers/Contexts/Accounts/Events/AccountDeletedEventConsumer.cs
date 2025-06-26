using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Events.Events;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events;

public sealed class AccountDeletedEventConsumer(
    IRepository<AccountReadModel> accountRepository,
    ILogger<AccountDeletedEventConsumer> logger) : IEventConsumer<AccountDeletedEvent>
{
    public async Task ConsumeAsync(AccountDeletedEvent @event, CancellationToken cancellationToken)
    {
        var account = await accountRepository
            .Where(a => a.Id == @event.AccountId)
            .SingleOrDefaultAsync(cancellationToken);

        if(account is null)
        {
            logger.LogWarning("Account with ID `{AccountId}` was not found.", @event.AccountId);
            throw new ReadableException($"Account with ID '{@event.AccountId}' was not found.");
        }

        await accountRepository.DeleteAsync(account, cancellationToken);
    }
}