using FluentValidation;
using SmartLedger.Common.Cqrs.Queries;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgetCategories;

/// <summary>
/// Validates <see cref="GetPaginatedBudgetsQuery"/> requests.
/// </summary>
public class GetPaginatedBudgetsQueryValidator : PaginatedQueryValidator<GetPaginatedBudgetsQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetPaginatedBudgetsQueryValidator()
    {
        RuleFor(q => q.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty.");
    }
}