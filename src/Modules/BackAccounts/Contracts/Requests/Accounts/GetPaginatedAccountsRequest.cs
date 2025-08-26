namespace SmartLedger.Modules.Transactions.Contracts.Requests.Accounts;

/// <summary>
/// Represents the request payload for retrieving a paginated list of accounts.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="MinBalance">Minimum transaction balance filter.</param>
/// <param name="MaxBalance">Maximum transaction balance filter.</param>
/// <param name="StartDate">Start date for filtering transactions.</param>
/// <param name="EndDate">End date for filtering transactions.</param>
public sealed record GetPaginatedAccountsRequest(
    int PageNumber,
    int PageSize,
    decimal? MinBalance,
    decimal? MaxBalance,
    DateTime? StartDate,
    DateTime? EndDate);