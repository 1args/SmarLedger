using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

public interface IReportsService
{
    Task<(Guid ReportId, string ReportPath)> GenerateReportAsync(ReportGenerationModel request, CancellationToken cancellationToken);

    Task<Stream> GetReportAsync(string reportPath, CancellationToken cancellationToken);


}