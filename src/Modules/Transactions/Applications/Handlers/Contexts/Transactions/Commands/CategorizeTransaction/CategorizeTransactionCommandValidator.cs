using FluentValidation;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Commands.CategorizeTransaction;

/// <summary>
/// Validates <see cref="CategorizeTransactionCommand"/> requests.
/// </summary>
public sealed class CategorizeTransactionCommandValidator : AbstractValidator<CategorizeTransactionCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CategorizeTransactionCommandValidator()
    {
        RuleFor(c => c.TransactionId)
            .NotEmpty().WithMessage("Transaction ID cannot be empty.");

        RuleFor(c => c.NewCategoryId)
            .NotEmpty().WithMessage("Category ID cannot be empty.");
    }
}