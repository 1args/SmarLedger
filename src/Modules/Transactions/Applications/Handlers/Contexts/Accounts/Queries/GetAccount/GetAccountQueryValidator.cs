using FluentValidation;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Queries.GetAccount;

/// <summary>
/// Validates <see cref="GetAccountQuery"/> requests.
/// </summary>
public sealed class GetAccountQueryValidator : AbstractValidator<GetAccountQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GetAccountQueryValidator()
    {
        RuleFor(q => q.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty.");
    }
}