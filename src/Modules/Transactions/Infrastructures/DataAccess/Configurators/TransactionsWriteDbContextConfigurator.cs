using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="TransactionsWriteDbContext"/>.
/// </summary>
public sealed class TransactionsWriteDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<TransactionsWriteDbContext>(configuration, loggerFactory);