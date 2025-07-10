using FluentValidation;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudget;

/// <summary>
/// Validates <see cref="DeleteBudgetCommand"/> requests.
/// </summary>
public sealed class DeleteBudgetCommandValidator : AbstractValidator<DeleteBudgetCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public DeleteBudgetCommandValidator()
    {
        RuleFor(c => c.BudgetId)
            .NotEmpty().WithMessage("Budget ID cannot be empty.");
    }
}