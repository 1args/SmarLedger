using FluentValidation;
using SmartLedger.Modules.Budgets.Domain.ValueObjects;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.CreateBudget;

/// <summary>
/// Validates <see cref="CreateBudgetCommand"/> requests.
/// </summary>
public sealed class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateBudgetCommandValidator()
    {
        RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Account name cannot be empty.")
            .MaximumLength(BudgetName.MaxLength)
            .WithMessage($"Account name cannot exceed '{BudgetName.MaxLength}' characters.");

        RuleFor(c => c.StartDate)
            .NotEmpty().WithMessage("Start date cannot be empty.");

        RuleFor(c => c.EndDate)
            .NotEmpty().WithMessage("End date cannot be empty.");
    }
}