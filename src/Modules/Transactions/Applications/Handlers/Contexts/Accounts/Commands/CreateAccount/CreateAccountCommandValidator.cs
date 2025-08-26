using FluentValidation;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.CreateAccount;

/// <summary>
/// Validates <see cref="CreateAccountCommand"/> requests.
/// </summary>
public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Account name cannot be empty.")
            .MaximumLength(AccountName.MaxLength)
            .WithMessage($"Account name cannot exceed '{AccountName.MaxLength}' characters.");
    }
}