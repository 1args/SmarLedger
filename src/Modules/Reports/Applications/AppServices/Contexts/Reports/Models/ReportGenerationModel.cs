using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;

public sealed record ReportGenerationModel(
    Guid UserId,
    ReportType Type,
    DateTime? StartPeriod,
    DateTime? EndPeriod);

