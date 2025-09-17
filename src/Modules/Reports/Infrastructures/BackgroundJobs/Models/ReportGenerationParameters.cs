using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Infrastructures.BackgroundJobs.Models;

/// <summary>
/// Parameters for report generation.
/// </summary>
/// <param name="Type">Report type.</param>
/// <param name="GeneratedAt">Date and time the report generated.</param>
/// <param name="UserId">User ID.</param>
/// <param name="StartPeriod">Start date and time.</param>
/// <param name="EndPeriod">End date and time.</param>
public sealed record ReportGenerationParameters(
    Guid ReportId,
    ReportType Type,
    DateTime GeneratedAt,
    Guid UserId,
    DateTime? StartPeriod,
    DateTime? EndPeriod);