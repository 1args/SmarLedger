using FluentValidation;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Queries.GetTransaction;

/// <summary>
/// Validates <see cref="GetTransactionQuery"/> requests.
/// </summary>
public sealed class GetTransactionQueryValidator : AbstractValidator<GetTransactionQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetTransactionQueryValidator()
    {
        RuleFor(q => q.TransactionId)
            .NotEmpty().WithMessage("Transaction ID cannot be empty.");
    }
}