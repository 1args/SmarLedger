using SmartLedger.Modules.Notifications.Contracts.Enums;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Templates.Services.Abstractions;

/// <summary>
/// Interface for retrieving notification template content.
/// </summary>
public interface ITemplateProvider
{
    /// <summary>
    /// Retrieves the template content (body and subject) for the given notification type.
    /// </summary>
    /// <param name="type">Type of notification.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <param name="data">Optional placeholder values to replace in the template.</param>
    /// <returns>Tuple containing the processed body and subject of the template.</returns>
    Task<(string body, string subject)> GetTemplateContentAsync(NotificationType type, CancellationToken cancellationToken, Dictionary<string, string>? data = null);
}