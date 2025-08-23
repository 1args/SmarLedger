using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Queries.GetReport;

// temp
public sealed record GetReportQuery(string ReportPath) : IQuery<Stream>;