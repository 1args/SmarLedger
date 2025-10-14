using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="BudgetsReadDbContext"/>.
/// </summary>
public sealed class BudgetsReadDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<BudgetsReadDbContext>(configuration, loggerFactory)
{
    /// <inheritdoc />
    protected override string ConnectionStringName => "Budgets";
}