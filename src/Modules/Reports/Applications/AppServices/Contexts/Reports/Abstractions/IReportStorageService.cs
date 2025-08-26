using SmartLedger.Modules.Reports.Domain.Aggregates;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

/// <summary>
/// Interface for report storing reports and information about them.
/// </summary>
public interface IReportStorageService
{
    /// <summary>
    /// Saves a report to the specified file path with the provided PDF stream.
    /// </summary>
    /// <param name="report">Report mode.</param>
    /// <param name="pdfStream">Stream containing a report.</param>
    /// <param name="reportPath">Report path.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SaveAsync(Report report, Stream pdfStream, string reportPath, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a stream of the report from the specified file path.
    /// </summary>
    /// <param name="reportId">Report ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns></returns>
    Task<Stream> GetReportStreamAsync(Guid reportId, CancellationToken cancellationToken);
}