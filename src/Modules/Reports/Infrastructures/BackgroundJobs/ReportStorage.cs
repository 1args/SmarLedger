using SmartLedger.Common.Contracts.Constants;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Common.Infrastructures.FileStorage.Abstractions;
using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs;

/// <summary>
/// Service for report storing reports and information about them.
/// </summary>
public sealed class ReportStorage(
    IMinioFileStorage minioFileStorage,
    IRepository<ReportDetailsModel, ReportsDbContext> reportsRepository): IReportStorage
{
    /// <inheritdoc/>
    public async Task SaveAsync(Report report, Stream pdfStream, string reportPath, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            minioFileStorage.UploadFileAsync(
                MinioBuckets.ReportsBucket,
                reportPath,
                "application/pdf",
                pdfStream,
                cancellationToken),
            SaveReportInfoAsync(report, reportPath, cancellationToken));
    }

    /// <summary>
    /// Saves report information to the database.
    /// </summary>
    private async Task SaveReportInfoAsync(Report report, string reportPath, CancellationToken cancellationToken)
    {
        var reportInfo = new ReportDetailsModel
        {
            Id = report.Id,
            UserId = report.UserInfo.UserId,
            Type = report.Type.ToString(),
            Path = reportPath,
            GeneratedAt = report.GeneratedAt,
        };

        await reportsRepository.AddAsync(reportInfo, cancellationToken);
    }
}