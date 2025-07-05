using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Specifications;

/// <summary>
/// Filters accounts by a minimum and/or maximum balance.
/// </summary>
/// <param name="minBalance">Optional minimum account balance.</param>
/// <param name="maxBalance">Optional maximum account balance.</param>
public sealed class AccountByBalanceRangeSpecification(decimal? minBalance, decimal? maxBalance) 
    : Specification<AccountReadModel>(
        a => (!minBalance.HasValue || a.Balance >= minBalance.Value) && 
             (!maxBalance.HasValue || a.Balance <= maxBalance.Value));