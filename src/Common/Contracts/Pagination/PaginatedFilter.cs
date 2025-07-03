namespace SmartLedger.Common.Contracts.Pagination;

/// <summary>
/// Filter for pagination.
/// </summary>
public record PaginatedFilter
{
    public const int MaxPageSize = 100;

    public const int MinPageSize = 0;

    private int _pageNumber = 1;

    private int _pageSize = 10;

    /// <summary>Page number (starting from 1).</summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value > MinPageSize ? value : 1;
    }

    /// <summary>Page size (number of items per page).</summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is > MinPageSize and <= MaxPageSize ? value : 10;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedFilter"/>.
    /// </summary>
    /// <param name="pageNumber">Page number (1 by default).</param>
    /// <param name="pageSize">Page size (default 10, maximum 100).</param>
    public PaginatedFilter(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}