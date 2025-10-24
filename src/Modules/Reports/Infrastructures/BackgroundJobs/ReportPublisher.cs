using System.Data;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Reports.Contracts.Common;
using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;
using SmartLedger.Modules.Webhooks.Contracts.Common;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Abstractions;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs;

/// <summary>
/// Service for publishing the generated report to the desired destination
/// </summary>
public sealed class ReportPublisher(
    IReportStorage reportStorage,
    IWebhooksDispatcher webhooksDispatcher,
    ITransactionManager transactionManager) : IReportPublisher
{
    /// <inheritdoc/>
    public async Task PublishAsync(Report report, Stream pdfStream, CancellationToken cancellationToken)
    {
        var filePath = string.Format(ReportPaths.ReportFilePathFormat, report.UserInfo.UserId, report.Id, report.Type);

        await transactionManager.StartEffectAsync(async ct =>
        {
            await reportStorage.SaveAsync(report, pdfStream, filePath, ct);
            await webhooksDispatcher.DispatchAsync(WebhookEvents.ReportGenerated, report.Id, ct);
        }, IsolationLevel.RepeatableRead, cancellationToken);
    }
}