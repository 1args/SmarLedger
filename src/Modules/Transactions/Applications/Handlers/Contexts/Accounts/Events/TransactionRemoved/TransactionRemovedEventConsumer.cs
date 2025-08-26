using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Events.TransactionRemoved;

/// <summary>
/// Consumes the <see cref="TransactionRemovedEvent"/>.
/// </summary>
public sealed class TransactionRemovedEventConsumer(
    IAccountsSynchronizationService accountsSynchronizationService,
    IDateTimeProvider dateTimeProvider) : IEventConsumer<TransactionRemovedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(TransactionRemovedEvent @event, CancellationToken cancellationToken)
    {
        var request = new TransactionRemovalSynchronizationModel(
            @event.TransactionId,
            dateTimeProvider.UtcNow);

        await accountsSynchronizationService.SynchronizeTransactionRemovalAsync(request, cancellationToken);
    }
}