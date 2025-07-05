using FluentValidation;
using SmartLedger.Common.Cqrs.Queries;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Queries.GetPaginatedAccounts;

/// <summary>
/// Validates <see cref="GetPaginatedAccountsQuery"/> requests.
/// </summary>
public sealed class GetPaginatedAccountsQueryValidator : PaginatedQueryValidator<GetPaginatedAccountsQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetPaginatedAccountsQueryValidator()
    {
        RuleFor(q => q.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty.");
    }
};