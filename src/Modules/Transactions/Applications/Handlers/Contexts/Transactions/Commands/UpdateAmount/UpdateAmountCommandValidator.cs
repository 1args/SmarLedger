using FluentValidation;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.UpdateAmount;

/// <summary>
/// Validates <see cref="UpdateAmountCommand"/> requests.
/// </summary>
public sealed class UpdateAmountCommandValidator : AbstractValidator<UpdateAmountCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public UpdateAmountCommandValidator()
    {
        RuleFor(c => c.TransactionId)
            .NotEmpty().WithMessage("Transaction ID cannot be empty.");

        RuleFor(c => c.NewAmount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Amount cannot be empty.")
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}