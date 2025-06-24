using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Cqrs.Query.Abstractions;

namespace SmartLedger.Common.Cqrs.Queries;

/// <summary>
/// Represents pagination parameters for queries that support paging.
/// </summary>
/// <typeparam name="TResponse">Response type.</typeparam>
/// <param name="pageNumber">Page number.</param>
/// <param name="pageSize">Page size.</param>
public class PaginatedQuery<TResponse>(
    int pageNumber,
    int pageSize)
    : IQuery<PaginatedList<TResponse>>, IPaginatedQuery
    where TResponse : class
{
    /// <inheritdoc />
    public int PageNumber { get; set; }

    /// <inheritdoc />
    public int PageSize { get; set; }
}