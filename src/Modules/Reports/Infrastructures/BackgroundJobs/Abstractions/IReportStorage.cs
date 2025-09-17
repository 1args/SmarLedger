using SmartLedger.Modules.Reports.Domain.Aggregates;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;

/// <summary>
/// Interface for report storing reports and information about them.
/// </summary>
public interface IReportStorage
{
    /// <summary>
    /// Saves a report to the specified file path with the provided PDF stream.
    /// </summary>
    /// <param name="report">Report mode.</param>
    /// <param name="pdfStream">Stream containing a report.</param>
    /// <param name="reportPath">Report path.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SaveAsync(Report report, Stream pdfStream, string reportPath, CancellationToken cancellationToken);
}