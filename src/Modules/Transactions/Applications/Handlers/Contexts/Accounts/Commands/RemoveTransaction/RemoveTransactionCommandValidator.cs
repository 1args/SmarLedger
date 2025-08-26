using FluentValidation;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.RemoveTransaction;

/// <summary>
/// Validates <see cref="RemoveTransactionCommand"/> requests.
/// </summary>
public class RemoveTransactionCommandValidator : AbstractValidator<RemoveTransactionCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public RemoveTransactionCommandValidator()
    {
        RuleFor(c => c.AccountId)
            .NotEmpty().WithMessage("Account ID cannot be empty.");

        RuleFor(c => c.TransactionId)
            .NotEmpty().WithMessage("Transaction ID cannot be empty");
    }
}