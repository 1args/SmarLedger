using FluentValidation;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateTransactionAmount;

/// <summary>
/// Validates <see cref="UpdateTransactionAmountCommand"/> requests.
/// </summary>
public sealed class UpdateTransactionAmountCommandValidator : AbstractValidator<UpdateTransactionAmountCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public UpdateTransactionAmountCommandValidator()
    {
        RuleFor(c => c.TransactionId)
            .NotEmpty().WithMessage("Transaction ID cannot be empty.");

        RuleFor(c => c.NewAmount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Amount cannot be empty.")
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}