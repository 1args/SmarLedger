using FluentValidation;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.DeleteTransaction;

/// <summary>
/// Validates <see cref="DeleteTransactionCommand"/> requests.
/// </summary>
public class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DeleteTransactionCommandValidator()
    {
        RuleFor(c => c.AccountId)
            .NotEmpty().WithMessage("Account ID cannot be empty.");

        RuleFor(c => c.TransactionId)
            .NotEmpty().WithMessage("Transaction ID cannot be empty");
    }
}