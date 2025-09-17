using SmartLedger.Modules.Reports.Domain.Aggregates;
using SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Models;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Abstractions;

/// <summary>
/// Factory interface for creating reports.
/// </summary>
public interface IReportFactory
{
    /// <summary>
    /// Creates a report based on the provided parameters.
    /// </summary>
    /// <param name="parameters">Parameters for report generation</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Report.</returns>
    Task<Report> CreateReportAsync(ReportGenerationParameters parameters, CancellationToken cancellationToken);
}