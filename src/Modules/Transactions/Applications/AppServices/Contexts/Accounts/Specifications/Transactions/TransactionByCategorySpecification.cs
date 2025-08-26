using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Specifications.Transactions;

/// <summary>
/// Filters transactions by category if specified.
/// </summary>
/// <param name="category">Optional category to filter transactions by.</param>
public sealed class TransactionByCategorySpecification(string? category)
    : Specification<TransactionReadModel>(t => string.IsNullOrEmpty(category) || t.Category == category);