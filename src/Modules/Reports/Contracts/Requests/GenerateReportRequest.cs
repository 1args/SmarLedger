using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Contracts.Requests;

/// <summary>
/// Represents a request to generate a new report.
/// </summary>
/// <param name="Type">Report type.</param>
/// <param name="StartPeriod"></param>
/// <param name="EndPeriod"></param>
public sealed record GenerateReportRequest(
    ReportType Type,
    DateTime? StartPeriod,
    DateTime? EndPeriod);