using Microsoft.Extensions.Options;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Factories.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Abstractions;
using SmartLedger.Modules.Notifications.Contracts.Options;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Factories;

/// <summary>
/// Resolves strategies from a registered collection
/// </summary>
public sealed class EmailSendingStrategiesFactory(
    IOptions<EmailSendingOptions> options, 
    IEnumerable<IEmailSendingStrategy> strategies) : IEmailSendingStrategiesFactory
{
    private readonly EmailSendingOptions _options = options.Value;

    private readonly Dictionary<string, IEmailSendingStrategy> _strategies = strategies
        .ToDictionary(s => s.Type, s => s, StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public IEmailSendingStrategy GetStrategy(string strategyType)
    {
        if(_strategies.TryGetValue(strategyType, out var strategy))
        {
            return strategy;
        }
        throw new NotSupportedException($"Email sending type {strategyType} is not supported");
    }
}