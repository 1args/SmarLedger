using SmartLedger.Common.Cqrs.Queries;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Accounts;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetPaginatedAccounts;

/// <summary>
/// Represents a query to retrieve paginated transactions with filter.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="MinBalance">Minimum transaction balance filter.</param>
/// <param name="MaxBalance">Maximum transaction balance filter.</param>
/// <param name="StartDate">Start date for filtering transactions.</param>
/// <param name="EndDate">End date for filtering transactions.</param>
public sealed record GetPaginatedAccountsQuery(
    int PageNumber,
    int PageSize,
    decimal? MinBalance,
    decimal? MaxBalance,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedQuery<AccountListItem>(PageNumber, PageSize);