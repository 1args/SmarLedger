using FluentValidation;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;

/// <summary>
/// Validates <see cref="AddTransactionCommand"/> requests.
/// </summary>
public sealed class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public AddTransactionCommandValidator()
    {
        RuleFor(c => c.AccountId)
            .NotEmpty().WithMessage("Account ID cannot be empty.");

        RuleFor(c => c.Amount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Amount cannot be empty.")
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(c => c.Type)
            .NotEmpty().WithMessage("Type cannot be empty");
    }
}