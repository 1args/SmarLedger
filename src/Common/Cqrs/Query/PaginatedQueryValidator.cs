using FluentValidation;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Cqrs.Query.Abstractions;

namespace SmartLedger.Common.Cqrs.Query;

/// <summary>
/// Base validator for paginated queries.
/// </summary>
/// <typeparam name="TResponse">Response type.</typeparam>
public abstract class PaginatedQueryValidator<TResponse> : AbstractValidator<TResponse>
    where TResponse : class, IPaginatedQuery
{
    /// <summary>
    /// Constructor.
    /// </summary>
    protected PaginatedQueryValidator()
    {
        RuleFor(response => response.PageNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Page number field cannot be empty.")
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(response => response.PageSize)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Page size field cannot be empty.")
            .GreaterThan(PaginatedFilter.MinPageSize)
            .WithMessage($"Page size must be greater than {PaginatedFilter.MinPageSize}.")
            .LessThanOrEqualTo(PaginatedFilter.MaxPageSize)
            .WithMessage($"Page size must not exceed maximum allowed value of {PaginatedFilter.MaxPageSize}.");
    }
}