using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Cqrs.Queries.Abstractions;

namespace SmartLedger.Common.Cqrs.Queries;

/// <summary>
/// Represents pagination parameters for queries that support paging.
/// </summary>
/// <typeparam name="TResponse">Response type.</typeparam>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
public record PaginatedQuery<TResponse>(int PageNumber, int PageSize)
    : IQuery<PaginatedList<TResponse>>, IPaginatedQuery
    where TResponse : class;