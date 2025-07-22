using FluentValidation;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategory;

/// <summary>
/// Validates <see cref="GetBudgetCategoryQuery"/> requests.
/// </summary>
public sealed class GetBudgetCategoryQueryValidator : AbstractValidator<GetBudgetCategoryQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetBudgetCategoryQueryValidator()
    {
        RuleFor(q => q.BudgetCategoryId)
            .NotEmpty().WithMessage("Budget category ID cannot be empty.");
    }
}