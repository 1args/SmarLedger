using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;

namespace SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="WebhooksDbContextConfigurator"/>.
/// </summary>
public sealed class WebhooksDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<WebhooksDbContext>(configuration, loggerFactory);