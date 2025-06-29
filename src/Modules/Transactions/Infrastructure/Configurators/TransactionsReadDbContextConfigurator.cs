using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;

namespace SmartLedger.Modules.Transactions.Infrastructure.Configurators;

/// <summary>
/// Configurator for <see cref="TransactionsReadDbContext"/>.
/// </summary>
public sealed class TransactionsReadDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<TransactionsReadDbContext>(configuration, loggerFactory);