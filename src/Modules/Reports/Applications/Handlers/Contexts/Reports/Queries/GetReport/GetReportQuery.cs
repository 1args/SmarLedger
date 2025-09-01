using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Queries.GetReport;

/// <summary>
/// Represents a query to retrieve a report by its ID.
/// </summary>
/// <param name="ReportId">Report ID.</param>
public sealed record GetReportQuery(Guid ReportId) : IQuery<Stream>;