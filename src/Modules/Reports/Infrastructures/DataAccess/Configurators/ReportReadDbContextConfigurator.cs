using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Modules.Reports.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="ReportReadDbContextConfigurator"/>.
/// </summary>
public sealed class ReportReadDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<ReportsReadDbContext>(configuration, loggerFactory);