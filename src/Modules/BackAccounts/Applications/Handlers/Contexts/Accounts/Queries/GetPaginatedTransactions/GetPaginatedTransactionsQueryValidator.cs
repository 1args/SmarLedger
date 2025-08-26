using FluentValidation;
using SmartLedger.Common.Cqrs.Queries;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetPaginatedTransactions;

/// <summary>
/// Validates <see cref="GetPaginatedTransactionsQuery"/> requests.
/// </summary>
public sealed class GetPaginatedTransactionsQueryValidator : PaginatedQueryValidator<GetPaginatedTransactionsQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetPaginatedTransactionsQueryValidator()
    {
        RuleFor(q => q.AccountId)
            .NotEmpty().WithMessage("Account ID cannot be empty.");

        RuleFor(q => q.MinAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum transaction amount must be greater than or equal to 0.");

        RuleFor(q => q.MaxAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maximum transaction amount must be greater than or equal to 0.");
    }
}