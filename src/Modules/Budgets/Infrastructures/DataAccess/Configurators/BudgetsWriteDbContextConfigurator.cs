using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="BudgetsWriteDbContext"/>.
/// </summary>
public sealed class BudgetsWriteDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<BudgetsWriteDbContext>(configuration, loggerFactory)
{
    /// <inheritdoc />
    protected override string ConnectionStringName => "Budgets";
}