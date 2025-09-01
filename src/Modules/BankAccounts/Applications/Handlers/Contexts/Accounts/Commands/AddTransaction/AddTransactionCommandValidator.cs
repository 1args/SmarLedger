using FluentValidation;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Commands.AddTransaction;

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
            .GreaterThan(0).WithMessage("Amount must be greater than 0.")
            .Must(amount => amount <= Money.MaxTransactionAmount)
            .WithMessage($"Amount cannot exceed {Money.MaxTransactionAmount:N0}."); ;

        RuleFor(c => c.Type)
            .IsInEnum().WithMessage("Select the correct transaction type from the available values.");

        RuleFor(c => c.Category)
            .IsInEnum().WithMessage("Select the correct transaction category from the available values.");

        RuleFor(c => c.Notes)
            .NotEmpty().WithMessage("Notes cannot be empty")
            .MaximumLength(TransactionDescription.MaxLength)
            .WithMessage($"Notes cannot exceed '{TransactionDescription.MaxLength}' characters.");
    }
}