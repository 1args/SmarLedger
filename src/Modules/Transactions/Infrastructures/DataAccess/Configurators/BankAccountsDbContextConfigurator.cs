using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Configurators;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read;

namespace SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Configurators;

/// <summary>
/// Configurator for <see cref="BackAccountsReadDbContext"/>.
/// </summary>
public sealed class BankAccountsDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : BaseDbContextConfigurator<BackAccountsReadDbContext>(configuration, loggerFactory);