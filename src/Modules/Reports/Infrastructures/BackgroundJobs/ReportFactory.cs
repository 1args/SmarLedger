using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write;
using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Domain.Enums;
using SmartLedger.Modules.Reports.Domain.ValueObjects;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Models;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs;

/// <summary>
/// Factory for creating reports.
/// </summary>
public sealed class ReportFactory(
    IKeycloakUserApiClient keycloakUserApiClient,
    IRepository<Account, BackAccountsWriteDbContext> accountsRepository,
    IRepository<Budget, BudgetsWriteDbContext> budgetsRepository) : IReportFactory
{
    /// <inheritdoc/>
    public async Task<Report> CreateReportAsync(ReportGenerationParameters parameters, CancellationToken cancellationToken)
    {
        var user = await keycloakUserApiClient.GetUserAsync(parameters.UserId, cancellationToken);
        var userInfo = UserInfo.Create(user.Id, user.Username, user.FirstName, user.LastName);
        var (accounts, budgets) = await GetBudgetsAndAccountsAsync(parameters.UserId, cancellationToken);

        return parameters.Type == ReportType.Custom
            ? Report.Create(
                parameters.ReportId,
                userInfo,
                accounts,
                budgets,
                parameters.Type,
                parameters.GeneratedAt)
            : Report.Create(
                parameters.ReportId,
                userInfo,
                accounts, 
                budgets,
                parameters.Type,
                parameters.GeneratedAt,
                parameters.StartPeriod,
                parameters.EndPeriod);
    }

    /// <summary>
    /// Retrieves the lists of accounts and budgets associated with the specified user.
    /// </summary>
    private async Task<(List<Account> Accounts, List<Budget> Budgets)> GetBudgetsAndAccountsAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var uid = UserId.Create(userId);

        var accounts = await accountsRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(a => a.UserId == uid)
            .Include(a => a.Transactions)
            .ToListAsync(cancellationToken);

        var budgets = await budgetsRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(b => b.UserId == uid)
            .Include(b => b.Categories)
            .ToListAsync(cancellationToken);

        return (accounts, budgets);
    }
}