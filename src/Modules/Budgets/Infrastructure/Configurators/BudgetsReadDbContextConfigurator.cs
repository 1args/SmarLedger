using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read;

namespace SmartLedger.Modules.Budgets.Infrastructure.Configurators;

/// <summary>
/// Configurator for <see cref="BudgetsReadDbContext"/>.
/// </summary>
public sealed class BudgetsReadDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<BudgetsReadDbContext>(configuration, loggerFactory);