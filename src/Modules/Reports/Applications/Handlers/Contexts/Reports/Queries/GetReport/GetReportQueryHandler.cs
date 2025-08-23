using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;

namespace SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Queries.GetReport;

// temp
public sealed class GetReportQueryHandler(
    IReportsService reportsService) : IQueryHandler<GetReportQuery, Stream>
{
    public async Task<Stream> HandleAsync(GetReportQuery query, CancellationToken cancellationToken)
    {
        return await reportsService.GetReportAsync(query.ReportPath, cancellationToken);
    }
}