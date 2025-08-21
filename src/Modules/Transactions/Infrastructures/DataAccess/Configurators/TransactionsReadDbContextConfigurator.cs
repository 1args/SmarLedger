using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="TransactionsReadDbContext"/>.
/// </summary>
public sealed class TransactionsReadDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<TransactionsReadDbContext>(configuration, loggerFactory);