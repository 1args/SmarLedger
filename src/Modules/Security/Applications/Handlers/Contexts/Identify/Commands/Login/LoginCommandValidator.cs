using FluentValidation;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Login;

/// <summary>
/// Validates <see cref="LoginCommand"/> requests.
/// </summary>
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty().WithMessage("User name cannot be empty.");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password cannot be empty.");
    }
}