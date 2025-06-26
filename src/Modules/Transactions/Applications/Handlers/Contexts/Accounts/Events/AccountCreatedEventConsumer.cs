using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Events.Events;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events;

public sealed class AccountCreatedEventConsumer(
    IRepository<AccountReadModel> accountRepository) : IEventConsumer<AccountCreatedEvent>
{
    public async Task ConsumeAsync(AccountCreatedEvent @event, CancellationToken cancellationToken)
    {
        var account = new AccountReadModel
        {
            Id = @event.AccountId,
            Name = @event.Name,
            Balance = 0.0m,
            CreatedAt = @event.CreatedAt,
            LastUpdatedAt = @event.CreatedAt
        };

        await accountRepository.AddAsync(account, cancellationToken);
    }
}