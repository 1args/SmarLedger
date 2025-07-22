using FluentValidation;
using SmartLedger.Common.Cqrs.Queries;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgets;

/// <summary>
/// Validates <see cref="GetPaginatedBudgetCategoriesQuery"/> requests.
/// </summary>
public sealed class GetPaginatedBudgetCategoriesQueryValidator : PaginatedQueryValidator<GetPaginatedBudgetCategoriesQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetPaginatedBudgetCategoriesQueryValidator()
    {
        RuleFor(q => q.BudgetId)
            .NotEmpty().WithMessage("Budget ID cannot be empty.");

        RuleFor(q => q.Category)
            .NotEmpty().WithMessage("Category cannot be empty.");
    }
}