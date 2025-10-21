using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

namespace SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Queries.GetReport;

/// <summary>
/// Handles the logic for processing <see cref="GetReportQuery"/>.
/// </summary>
/// <param name="reportsService"></param>
public sealed class GetReportQueryHandler(
    IReportsService reportsService) : IQueryHandler<GetReportQuery, Stream>
{
    /// <inheritdoc />
    public async Task<Stream> HandleAsync(GetReportQuery query, CancellationToken cancellationToken)
    {
        return await reportsService.GetReportAsync(query.ReportId, cancellationToken);
    }
}