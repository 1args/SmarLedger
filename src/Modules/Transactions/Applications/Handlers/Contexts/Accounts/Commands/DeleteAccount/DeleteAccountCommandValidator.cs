using FluentValidation;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Commands.DeleteAccount;

/// <summary>
/// Validates <see cref="DeleteAccountCommand"/> requests.
/// </summary>
public sealed class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public DeleteAccountCommandValidator()
    {
        RuleFor(c => c.AccountId)
            .NotEmpty().WithMessage("Account ID cannot be empty.");
    }
}