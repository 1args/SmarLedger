using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Commands.GenerateReport;

// temp
public sealed record GenerateReportCommand(
    ReportType Type,
    DateTime? StartPeriod,
    DateTime? EndPeriod) : ICommand<Guid>;
