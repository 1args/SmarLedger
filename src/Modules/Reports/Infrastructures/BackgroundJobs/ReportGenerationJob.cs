using Hangfire;
using Microsoft.Extensions.Logging;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Models;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs;

/// <summary>
/// Represents a background job for generating reports.
/// </summary>
public sealed class ReportGenerationJob(
    IReportFactory reportFactory,
    IPdfGenerator pdfGenerator,
    IReportPublisher reportPublisher,
    IReportTemplateProvider reportTemplateProvider,
    ILogger<ReportGenerationJob> logger)
{
    /// <summary>
    /// Executes the report generation workflow asynchronously using the specified parameters.
    /// </summary>
    /// <param name="parameters">Parameters for report generation</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    [JobDisplayName("Generate Report")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [60, 120, 300])]
    public async Task ExecuteAsync(ReportGenerationParameters parameters, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting report generation job for user with ID {UserId} of type {ReportType}",
            parameters.UserId, parameters.Type);

        var report = await reportFactory.CreateReportAsync(parameters, cancellationToken);
        var htmlBody = await reportTemplateProvider.CompileTemplateAsync(report, cancellationToken);

        await using var pdfStream = await pdfGenerator.GeneratePdfAsync(htmlBody, cancellationToken);

        await reportPublisher.PublishAsync(report, pdfStream, cancellationToken);

        logger.LogInformation(
            "Report generation job completed successfully with report ID {ReportId} for user with ID {UserId}",
            report.Id, parameters.UserId);
    }
}