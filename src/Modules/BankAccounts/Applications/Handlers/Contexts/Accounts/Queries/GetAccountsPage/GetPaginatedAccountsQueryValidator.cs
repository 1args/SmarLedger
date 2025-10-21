using FluentValidation;
using SmartLedger.Common.Cqrs.Queries;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetAccountsPage;

/// <summary>
/// Validates <see cref="GetAccountsPageQuery"/> requests.
/// </summary>
public sealed class GetPaginatedAccountsQueryValidator : PaginatedQueryValidator<GetAccountsPageQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetPaginatedAccountsQueryValidator()
    {
        RuleFor(q => q.MinBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum balance must be greater than or equal to 0.");

        RuleFor(q => q.MaxBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maximum balance must be greater than or equal to 0.");
    }
}