using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Modules.Reports.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="ReportsDbContextConfigurator"/>.
/// </summary>
public sealed class ReportsDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<ReportsDbContext>(configuration, loggerFactory)
{
    /// <inheritdoc />
    protected override string ConnectionStringName => "Reports";
}