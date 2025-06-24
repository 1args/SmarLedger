namespace SmartLedger.Common.Cqrs.Query.Abstractions;

/// <summary>
/// Defines pagination parameters for queries that support paging.
/// </summary>
public interface IPaginatedQuery
{
    /// <summary>
    /// Page number for the query.
    /// </summary>
    int PageNumber { get; set; }

    /// <summary>
    /// Page size for the query (number of items per page).
    /// </summary>
    int PageSize { get; set; }
}