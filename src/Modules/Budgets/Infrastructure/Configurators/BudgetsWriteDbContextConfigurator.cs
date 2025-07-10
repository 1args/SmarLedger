using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Write;

namespace SmartLedger.Modules.Budgets.Infrastructure.Configurators;

/// <summary>
/// Configurator for <see cref="BudgetsWriteDbContext"/>.
/// </summary>
public sealed class BudgetsWriteDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<BudgetsWriteDbContext>(configuration, loggerFactory);