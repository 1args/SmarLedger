namespace SmartLedger.Common.Cqrs.Queries.Abstractions;

/// <summary>
/// Defines pagination parameters for queries that support paging.
/// </summary>
public interface IPaginatedQuery
{
    /// <summary>
    /// Page number for the query.
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    /// Page size for the query (number of items per page).
    /// </summary>
    int PageSize { get; }
}