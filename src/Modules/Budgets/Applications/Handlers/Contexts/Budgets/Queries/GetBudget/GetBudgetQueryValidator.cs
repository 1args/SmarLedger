using FluentValidation;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudget;

/// <summary>
/// Validates <see cref="GetBudgetQuery"/> requests.
/// </summary>
public sealed class GetBudgetQueryValidator : AbstractValidator<GetBudgetQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetBudgetQueryValidator()
    {
        RuleFor(q => q.BudgetId)
            .NotEmpty().WithMessage("Budget ID cannot be empty.");
    }
}