using FluentValidation;
using SmartLedger.Modules.Security.Contracts.Common;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.ResetPassword;

/// <summary>
/// Validates <see cref="ResetPasswordCommand"/> requests.
/// </summary>
public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.CurrentPassword)
            .NotEmpty().WithMessage("Current password cannot be empty.");

        RuleFor(c => c.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("New password cannot be empty.")
            .MinimumLength(ValidationConstants.PasswordMinLength)
            .WithMessage($"Password must be at least '{ValidationConstants.PasswordMinLength}' characters.")
            .MaximumLength(ValidationConstants.PasswordMaxLength)
            .WithMessage($"Password length cannot exceed '{ValidationConstants.PasswordMaxLength}' characters.");
    }
}