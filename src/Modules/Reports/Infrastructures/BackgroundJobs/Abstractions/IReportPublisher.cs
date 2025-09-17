using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Models;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;

/// <summary>
/// Interface for publishing the generated report to the desired destination.
/// </summary>
public interface IReportPublisher
{
    /// <summary>
    /// Publishes the generated report to the desired destination.
    /// </summary>
    /// <param name="report">Report.</param>
    /// <param name="pdfStream">Pdf stream.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task PublishAsync(Report report, Stream pdfStream, CancellationToken cancellationToken);
}