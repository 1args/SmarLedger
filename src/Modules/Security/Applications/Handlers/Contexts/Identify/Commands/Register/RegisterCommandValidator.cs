using FluentValidation;
using SmartLedger.Modules.Security.Contracts.Common;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Register;

/// <summary>
/// Validates <see cref="RegisterCommand"/> requests.
/// </summary>
public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .Length(ValidationConstants.UsernameMinLength, ValidationConstants.UsernameMaxLength)
            .WithMessage($"Username must be between {ValidationConstants.UsernameMinLength} and {ValidationConstants.UsernameMaxLength} characters.");

        RuleFor(c => c.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(c => c.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .Length(ValidationConstants.FirstNameMinLength, ValidationConstants.FirstNameMaxLength)
            .WithMessage($"First name must be between {ValidationConstants.FirstNameMinLength} and {ValidationConstants.FirstNameMaxLength} characters.");

        RuleFor(c => c.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Second name cannot be empty..")
            .Length(ValidationConstants.SecondNameMinLength, ValidationConstants.SecondNameMaxLength)
            .WithMessage($"Second name must be between {ValidationConstants.SecondNameMinLength} and {ValidationConstants.SecondNameMaxLength} characters.");

        RuleFor(c => c.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .Length(ValidationConstants.PasswordMinLength, ValidationConstants.PasswordMaxLength)
            .WithMessage($"Password must be between {ValidationConstants.PasswordMinLength} and {ValidationConstants.PasswordMaxLength} characters.");
    }
}