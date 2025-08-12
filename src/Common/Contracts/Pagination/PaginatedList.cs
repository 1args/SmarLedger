using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace SmartLedger.Common.Contracts.Pagination;

/// <summary>
/// Presentation of a paginated list of data with metadata.
/// </summary>
/// <typeparam name="TData">Data type in the list.</typeparam>
public sealed class PaginatedList<TData>
{
    /// <summary>Total number of items.</summary>
    public int TotalCount { get; }

    /// <summary>Current page number (starting from 1).</summary>
    public int PageNumber { get; }

    /// <summary>Page size (number of items per page).</summary>
    public int PageSize { get; }

    /// <summary>Total number of pages.</summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>Indicates whether there is a previous page.</summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>Indicates whether there is a next page.</summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>List of data for current page.</summary>
    public List<TData> Items { get; }

    /// <summary>
    /// Private constructor used by the factory method.
    /// </summary>
    [JsonConstructor]
    private PaginatedList(int totalCount, int pageNumber, int pageSize, List<TData> items)
    {
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }

    /// <summary>
    /// Factory method for creating a paginated list.
    /// </summary>
    /// <param name="source">Data source to paginate.</param>
    /// <param name="paginationFilter">Pagination filter.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>New instance of the <see cref="PaginatedFilter"/> class.</returns>
    public static async Task<PaginatedList<TData>> CreateAsync(
        IQueryable<TData> source,
        PaginatedFilter paginationFilter,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TData>(
            totalCount,
            paginationFilter.PageNumber,
            paginationFilter.PageSize,
            items);
    }
}