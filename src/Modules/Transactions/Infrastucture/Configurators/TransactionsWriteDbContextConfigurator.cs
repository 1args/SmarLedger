using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructure.Configurators;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Write;

namespace SmartLedger.Modules.Transactions.Infrastructure.Configurators;

public sealed class TransactionsWriteDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<TransactionsWriteDbContext>(configuration, loggerFactory)
{
    protected override string ConnectionStringName => "DefaultConnection";
}