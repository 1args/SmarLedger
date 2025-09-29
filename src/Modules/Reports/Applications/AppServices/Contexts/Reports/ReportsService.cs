using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Constants;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Common.Infrastructures.FileStorage.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Models;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Reports.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports;

/// <inheritdoc/>
public sealed class ReportsService(
    Lazy<IAuthorizationData> authorizationData,
    IMinioFileStorage minioFileStorage,
    IRepository<ReportReadModel, ReportsReadDbContext> reportsRepository,
    IBackgroundJobClient backgroundJobClient,
    ILogger<ReportsService> logger) : IReportsService
{
    /// <inheritdoc/>
    public ValueTask<Guid> GenerateReport(ReportGenerationModel request, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation(
            "Enqueuing report generation job for user with ID {UserId} of type {ReportType}", 
            userId, request.Type);

        var parameters = new ReportGenerationParameters(
            Guid.NewGuid(),
            request.Type,
            request.GeneratedAt,
            userId,
            request.StartPeriod,
            request.EndPeriod);

        backgroundJobClient.Enqueue<ReportGenerationJob>(
            job => job.ExecuteAsync(parameters, cancellationToken));

        return ValueTask.FromResult(parameters.ReportId);
    }

    /// <inheritdoc/>
    public async Task<Stream> GetReportAsync(Guid reportId, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Retrieving report for user with ID {UserId} from path {ReportPath}", userId, reportId);

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