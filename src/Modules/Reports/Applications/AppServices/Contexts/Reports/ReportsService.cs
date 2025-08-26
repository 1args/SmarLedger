using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Write;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;
using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Domain.Enums;
using SmartLedger.Modules.Reports.Domain.ValueObjects;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;

/// <inheritdoc/>
public sealed class ReportsService(
    IKeycloakUserApiClient keycloakUserApiClient,
    IRepository<Account, BackAccountsWriteDbContext> accountsRepository,
    IRepository<Budget, BudgetsWriteDbContext> budgetsRepository,
    Lazy<IAuthorizationData> authorizationData,
    IReportTemplateService templateService,
    IPdfGenerator pdfGenerator,
    IReportStorageService reportStorage,
    ILogger<ReportsService> logger) : IReportsService
{
    /// <inheritdoc/>
    public async Task<(Guid ReportId, string ReportPath)> GenerateReportAsync(ReportGenerationModel request, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Generating report for user with ID {UserId} of type {ReportType}", userId, request.Type);

        var template = await templateService.CompileTemplateAsync("report.template", cancellationToken);

        var user = await keycloakUserApiClient.GetUserAsync(userId, cancellationToken);
        var userInfo = UserInfo.Create(user.Id, user.Username, user.FirstName, user.LastName);

        var (accounts, budgets) = await GetBudgetsAndAccountsAsync(userId, cancellationToken);

        var report = request.Type == ReportType.Custom
            ? Report.Create(userInfo, accounts, budgets, request.Type, request.GeneratedAt, request.StartPeriod, request.EndPeriod)
            : Report.Create(userInfo, accounts, budgets, request.Type, request.GeneratedAt);

        var htmlBody = template(report);
        await using var pdfStream = await pdfGenerator.GeneratePdfAsync(htmlBody, cancellationToken);

        var filePath = $"reports/{user.Id}/{report.Id}_{request.Type}.pdf";

        await reportStorage.SaveAsync(report, pdfStream, filePath, cancellationToken);

        logger.LogInformation("Report generated successfully with ID {ReportId} for user with ID {UserId}", report.Id, user.Id);

        return (report.Id, filePath);
    }

    /// <inheritdoc/>
    public async Task<Stream> GetReportAsync(Guid reportId, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Retrieving report for user with ID {UserId} from path {ReportPath}", userId, reportId);

        return await reportStorage.GetReportStreamAsync(reportId, cancellationToken);
    }

    /// <summary>
    /// Retrieves accounts and budgets for the specified user ID.
    /// </summary>
    private async Task<(IReadOnlyCollection<Account> Accounts, IReadOnlyCollection<Budget> Budgets)> GetBudgetsAndAccountsAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var accounts = await accountsRepository
            .Where(a => a.UserId == userId)
            .Include(a => a.Transactions)
            .ToListAsync(cancellationToken);

        var budgets = await budgetsRepository
            .Where(b => b.UserId == userId)
            .Include(b => b.Categories)
            .ToListAsync(cancellationToken);

        return (accounts, budgets);
    }
}