using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;

public sealed record ReportGenerationModel(
    ReportType Type,
    DateTime GeneratedAt,
    DateTime? StartPeriod,
    DateTime? EndPeriod);

