using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Reports.Domain.Enums;

namespace SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Commands.GenerateReport;

/// <summary>
/// Represents a command to generate a new report.
/// </summary>
/// <param name="Type">Report type.</param>
/// <param name="StartPeriod">Start date and time.</param>
/// <param name="EndPeriod">End date and time.</param>
public sealed record GenerateReportCommand(
    ReportType Type,
    DateTime? StartPeriod,
    DateTime? EndPeriod) : ICommand<Guid>;
