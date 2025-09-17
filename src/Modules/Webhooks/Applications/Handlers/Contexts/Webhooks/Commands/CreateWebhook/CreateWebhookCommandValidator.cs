using FluentValidation;
using SmartLedger.Modules.Webhooks.Contracts.Common;

namespace SmartLedger.Modules.Webhooks.Applications.Handlers.Contexts.Webhooks.Commands.CreateWebhook;

/// <summary>
/// Validates <see cref="CreateWebhookCommand"/> requests.
/// </summary>
public sealed class CreateWebhookCommandValidator : AbstractValidator<CreateWebhookCommand>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateWebhookCommandValidator()
    {
        RuleFor(w => w.EventType)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Event type cannot be empty.")
            .Length(ValidationConstants.EventTypeMinLength, ValidationConstants.EventTypeMaxLength)
            .WithMessage($"Event type must be between {ValidationConstants.EventTypeMinLength} and {ValidationConstants.EventTypeMaxLength} characters.");

        RuleFor(w => w.CallbackUrl)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Callback URL cannot be empty.")
            .MaximumLength(ValidationConstants.CallbackUrlMaxLength)
            .WithMessage($"Callback URL must not exceed {ValidationConstants.CallbackUrlMaxLength} characters.")
            .Must(BeValidUrl)
            .WithMessage("Callback URL must be a valid URL.");
    }

    /// <summary>
    /// Validates if the provided string is a well-formed URL.
    /// </summary>
    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}