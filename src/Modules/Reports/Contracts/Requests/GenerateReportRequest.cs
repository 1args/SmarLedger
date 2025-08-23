using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Contracts.Requests;

public sealed record GenerateReportRequest(
    ReportType Type,
    DateTime? StartPeriod,
    DateTime? EndPeriod);