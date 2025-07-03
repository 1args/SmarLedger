using FluentValidation;
using SmartLedger.Common.Cqrs.Queries;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Queries.GetPaginatedTransactions;

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
    }
}