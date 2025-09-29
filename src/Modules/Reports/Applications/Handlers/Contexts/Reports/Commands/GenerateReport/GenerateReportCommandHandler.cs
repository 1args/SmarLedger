using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Abstractions;
using SmartLedger.Modules.Reports.Applications.AppServices.Contexts.Reports.Models;

namespace SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Commands.GenerateReport;

/// <summary>
/// Handles the logic for processing <see cref="GenerateReportCommand"/>.
/// </summary>
public sealed class GenerateReportCommandHandler(
    IReportsService reportsService,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<GenerateReportCommand, Guid>
{
    /// <inheritdoc />
    public Task<Guid> HandleAsync(GenerateReportCommand command, CancellationToken cancellationToken)
    {
       var request = new ReportGenerationModel(
           command.Type,
           dateTimeProvider.UtcNow,
           command.StartPeriod,
           command.EndPeriod);

        return reportsService.GenerateReport(request, cancellationToken).AsTask();
    }
}