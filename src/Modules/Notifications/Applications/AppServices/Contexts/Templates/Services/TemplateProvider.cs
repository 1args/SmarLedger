using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Templates.Services.Abstractions;
using SmartLedger.Modules.Notifications.Contracts.Common.Templates;
using SmartLedger.Modules.Notifications.Contracts.Enums;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Templates.Services;

/// <summary>
/// Service for retrieving notification template content.
/// </summary>
public sealed class TemplateProvider : ITemplateProvider
{
    /// <inheritdoc />
    public async Task<(string body, string subject)> GetTemplateContentAsync(
        NotificationType type,
        CancellationToken cancellationToken,
        Dictionary<string, string>? data = null)
    {
        var (templateFileName, templateSubject) = type switch
        {
            NotificationType.EmailConfrimed => (TemplatePaths.EmailConfirmationPath, TemplateSubjects.EmailConfirmed),
            NotificationType.LimitExceeded => (TemplatePaths.LimitExceededPath, TemplateSubjects.LimitExceeded),
            _ => throw new NotSupportedException($"Notification type {type} is not supported.")
        };

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), templateFileName);
        var templateBody = await File.ReadAllTextAsync(templatePath, cancellationToken);
        var processedBody = ReplacePlaceholders(templateBody, data ?? []);

        return (processedBody, templateSubject);
    }

    /// <summary>
    /// Replaces placeholders in the specified template string with corresponding values from the provided dictionary.
    /// </summary>
    private string ReplacePlaceholders(string template, Dictionary<string, string> data)
    {
        var result = template;

        foreach (var pair in data)
        {
            result = result.Replace($"{{{{{pair.Key}}}}}", pair.Value);
        }

        return result;
    }
}