using FluentValidation;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.DeleteBudgetCategory;

/// <summary>
/// Validates <see cref="RemoveCategoryCommand"/> requests.
/// </summary>
public sealed class RemoveCommandHandlerValidator : AbstractValidator<RemoveCategoryCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public RemoveCommandHandlerValidator()
    {
        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("Category ID cannot be empty.");
    }
}