using System.Net;
using FluentValidation;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken;

/// <summary>
/// Validates <see cref="RefreshTokenQuery"/> requests.
/// </summary>
public sealed class RefreshTokenQueryValidator : AbstractValidator<RefreshTokenQuery>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public RefreshTokenQueryValidator()
    {
        RuleFor(q => q.RefreshToken)
            .NotEmpty().WithMessage("Refresh token cannot be empty.")
            .WithErrorCode(HttpStatusCode.Unauthorized.ToString());
    }
}