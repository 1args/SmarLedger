using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

/// <summary>
/// Provides functionality for managing reports.
/// </summary>
public interface IReportsService
{
    /// <summary>
    /// Generates a report based on the provided request model.
    /// </summary>
    /// <param name="request">Model containing report generation data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Report ID and path.</returns>
    Task<(Guid ReportId, string ReportPath)> GenerateReportAsync(ReportGenerationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a report. 
    /// </summary>
    /// <param name="reportId">Report ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Stream containing downloaded report.</returns>
    Task<Stream> GetReportAsync(Guid reportId, CancellationToken cancellationToken);
}