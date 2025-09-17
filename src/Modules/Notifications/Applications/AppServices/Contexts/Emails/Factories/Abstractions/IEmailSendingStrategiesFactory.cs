using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Abstractions;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Factories.Abstractions;

/// <summary>
/// Defines a factory for resolving <see cref="IEmailSendingStrategy"/> instances
/// based on a specified strategy type.
/// </summary>
public interface IEmailSendingStrategiesFactory
{
    /// <summary>
    /// Retrieves the email sending strategy associated with the given type.
    /// </summary>
    /// <param name="strategyType">Type of the strategy to retrieve.</param>
    /// <returns>Corresponding <see cref="IEmailSendingStrategy"/>.</returns>
    IEmailSendingStrategy GetStrategy(string strategyType);
}
