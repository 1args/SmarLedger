using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Contracts.Mappers;

/// <summary>
/// Account mapper for converting read models to response models.
/// </summary>
public static class AccountMapper
{
    /// <summary>
    /// Maps an <see cref="AccountReadModel"/> to an <see cref="AccountResponse"/>.
    /// </summary>
    /// <param name="account">Account read model to map.</param>
    /// <returns><see cref="AccountResponse"/> containing the mapped account data.</returns>
    public static AccountResponse MapToResponse(this AccountReadModel account) =>
        new(account.Id,
            account.UserId,
            account.Name,
            account.Balance,
            account.CreatedAt,
            account.LastUpdatedAt);

    /// <summary>
    /// Maps an <see cref="AccountReadModel"/> to an <see cref="AccountListItem"/>.
    /// </summary>
    /// <param name="account">Account read model to map.</param>
    /// <returns><see cref="AccountListItem"/> containing the mapped account data.</returns>
    public static AccountListItem MapToListItem(this AccountReadModel account) =>
        new(account.Id,
            account.Name,
            account.Balance,
            account.CreatedAt,
            account.LastUpdatedAt);
}