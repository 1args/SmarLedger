using FluentValidation;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudgetCategory;

/// <summary>
/// Validates <see cref="CreateBudgetCategoryCommand"/> requests.
/// </summary>
public sealed class CreateBudgetCategoryCommandValidator : AbstractValidator<CreateBudgetCategoryCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateBudgetCategoryCommandValidator()
    {
        RuleFor(c => c.BudgetId)
            .NotEmpty().WithMessage("Budget ID cannot be empty.");

        RuleFor(c => c.Category)
            .IsInEnum().WithMessage("Select the correct budget category from the available values.");

        RuleFor(c => c.Limit)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Limit cannot be empty.")
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}