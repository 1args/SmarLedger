using FluentValidation;
using SmartLedger.Common.Cqrs.Queries;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategoriesPage;

/// <summary>
/// Validates <see cref="GetBudgetCategoriesPageQuery"/> requests.
/// </summary>
public sealed class GetBudgetCategoriesPageQueryValidator : PaginatedQueryValidator<GetBudgetCategoriesPageQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetBudgetCategoriesPageQueryValidator()
    {
        RuleFor(q => q.BudgetId)
            .NotEmpty().WithMessage("Budget ID cannot be empty.");
    }
}