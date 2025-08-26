using Microsoft.EntityFrameworkCore;
using SmartLedger.Common.Contracts.Constants;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Common.Infrastructures.FileStorage.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;

/// <summary>
/// Service for report storing reports and information about them.
/// </summary>
public sealed class ReportStorageService(
    IMinioFileStorage minioFileStorage,
    IRepository<ReportReadModel, ReportsReadDbContext> reportsRepository): IReportStorageService
{
    /// <inheritdoc/>
    public async Task SaveAsync(Report report, Stream pdfStream, string filePath, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            minioFileStorage.UploadFileAsync(
                MinioBuckets.ReportsBucket,
                filePath,
                "application/pdf",
                pdfStream,
                cancellationToken),
            SaveReportInfoAsync(report, filePath, cancellationToken));
    }

    /// <summary>
    /// Saves report information to the database.
    /// </summary>
    private async Task SaveReportInfoAsync(Report report, string reportPath, CancellationToken cancellationToken)
    {
        var reportInfo = new ReportReadModel
        {
            Id = report.Id,
            UserId = report.UserInfo.UserId,
            Type = report.Type.ToString(),
            Path = reportPath,
            GeneratedAt = report.GeneratedAt,
        };

        await reportsRepository.AddAsync(reportInfo, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Stream> GetReportStreamAsync(Guid reportId, CancellationToken cancellationToken)
    {
        var reportInfo = await reportsRepository
            .Where(r => r.Id == reportId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Report with ID '{reportId}' was not found.");

        return await minioFileStorage.DownloadFileAsync(
            MinioBuckets.ReportsBucket,
            reportInfo.Path,
            cancellationToken);
    }
}