using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="BackAccountsWriteDbContext"/>.
/// </summary>
public sealed class BankAccountsWriteDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<BackAccountsWriteDbContext>(configuration, loggerFactory);