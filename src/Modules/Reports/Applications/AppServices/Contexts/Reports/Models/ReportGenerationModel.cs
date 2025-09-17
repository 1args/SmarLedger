using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;

/// <summary>
/// Model representing the generation of a report.
/// </summary>
/// <param name="Type">Report type.</param>
/// <param name="GeneratedAt">Date and time the report generated.</param>
/// <param name="StartPeriod">Start date and time.</param>
/// <param name="EndPeriod">End date and time.</param>
public sealed record ReportGenerationModel(
    ReportType Type,
    DateTime GeneratedAt,
    DateTime? StartPeriod,
    DateTime? EndPeriod);