using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts;

public sealed class AccountsRetrievalService(
    IRepository<AccountReadModel, TransactionsReadDbContext> accountsRepository,
    IRepository<TransactionReadModel, TransactionsReadDbContext> transactionsRepository,
    ILogger<AccountsRetrievalService> logger) : IAccountsRetrievalService
{

}