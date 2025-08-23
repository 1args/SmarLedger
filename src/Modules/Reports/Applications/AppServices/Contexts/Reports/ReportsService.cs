using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Constants;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Common.Infrastructures.FileStorage.Abstractions;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Write;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;
using SmartLedger.Modules.Reports.Contracts.Helpers;
using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Domain.Enums;
using SmartLedger.Modules.Reports.Domain.ValueObjects;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Aggregates;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Write;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;

/// <inheritdoc/>
public sealed class ReportsService(
    IKeycloakUserApiClient keycloakUserApiClient,
    IRepository<Account, TransactionsWriteDbContext> accountsRepository,
    IRepository<Budget, BudgetsWriteDbContext> budgetsRepository,
    IMinioFileStorage minioFileStorage,
    Lazy<IAuthorizationData> authorizationData,
    ILogger<ReportsService> logger) : IReportsService
{
    /// <inheritdoc/>
    public async Task<(Guid ReportId, string ReportPath)> GenerateReportAsync(ReportGenerationModel request, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Generating report for user with ID {UserId} of type {ReportType}", userId, request.Type);

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "report.template.hbs");
        var templateContent = await File.ReadAllTextAsync(templatePath, cancellationToken);

        var handlebars = Handlebars.Create();
        handlebars.RegisterHelpers();

        var template = Handlebars.Compile(templateContent);

        var user = await keycloakUserApiClient.GetUserAsync(userId, cancellationToken);
        var userInfo = UserInfo.Create(user.Id, user.Username, user.FirstName, user.LastName);

        var accounts = await accountsRepository
            .Where(a => a.UserId == userId)
            .Include(a => a.Transactions)
            .ToListAsync(cancellationToken);

        var budgets = await budgetsRepository
            .Where(b => b.UserId == userId)
            .Include(b => b.Categories)
            .ToListAsync(cancellationToken);

        var report = request.Type == ReportType.Custom
            ? Report.Create(userInfo, accounts, budgets, request.Type, request.GeneratedAt, request.StartPeriod, request.EndPeriod)
            : Report.Create(userInfo, accounts, budgets, request.Type, request.GeneratedAt);

       var htmlBody = template(report);

       await using var pdfStream = await GeneratePdfAsync(htmlBody);

       var filePath = $"reports/{user.Id}/{report.Id}_{request.Type}.pdf";

       await minioFileStorage.UploadFileAsync(
           MinioBuckets.ReportsBucket,
           filePath,
           "application/pdf",
           pdfStream,
           cancellationToken);

        logger.LogInformation("Report generated successfully with ID {ReportId} for user with ID {UserId}", report.Id, user.Id);

        return (report.Id, filePath);
    }

    /// <inheritdoc/>
    public async Task<Stream> GetReportAsync(string reportPath, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Retrieving report for user with ID {UserId} from path {ReportPath}", userId, reportPath);

        return await minioFileStorage.DownloadFileAsync(
            MinioBuckets.ReportsBucket,
            reportPath,
            cancellationToken);
    }

    /// <summary>
    /// Generates a PDF document from the provided HTML content.
    /// </summary>
    private async Task<Stream> GeneratePdfAsync(string htmlBody)
    {
        logger.LogInformation("Generating PDF from HTML content");

        try
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = ["--no-sandbox", "--disable-setuid-sandbox"]
            });

            await using var page = await browser.NewPageAsync();

            await page.SetContentAsync(htmlBody, new NavigationOptions
            {
                WaitUntil = [WaitUntilNavigation.Load, WaitUntilNavigation.Networkidle0]
            });

            var pddOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                PreferCSSPageSize = true,
                MarginOptions = new MarginOptions
                {
                    Top = "40px",
                    Right = "20px",
                    Bottom = "20px",
                    Left = "20px"
                }
            };

            var pdfStream = await page.PdfStreamAsync(pddOptions);
            pdfStream.Position = 0;

            logger.LogInformation("PDF generated successfully");

            return pdfStream;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while generating PDF from HTML content");
            throw new InvalidOperationException("Failed to generate PDF from HTML content.", ex);
        }
    }
}