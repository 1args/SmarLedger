using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Commands.GenerateReport;
using SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Queries.GetReport;
using SmartLedger.Modules.Reports.Contracts.Requests;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Abstractions;

namespace SmartLedger.Hosts.Api.Endpoints;

/// <summary>
/// Maps endpoints related to reports operations.
/// </summary>
public static class ReportsEndpoints
{
    /// <summary>
    /// Registers all report-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapReportsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/reports")
            .RequireAuthorization()
            .WithTags("Reports")
            .WithOpenApi();

        endpoints.MapPost("/generate", GenerateReportAsync)
            .WithName("GenerateReport")
            .WithSummary("Generates a new report.")
            .WithDescription("Creates a new report for the specified period and type. Returns the unique report identifier.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapGet("/{reportId:guid}", GetReportAsync)
            .WithName("GetReport")
            .WithSummary("Retrieves a report by its identifier.")
            .WithDescription("Fetches the generated report in PDF format using its unique identifier.")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    /// <summary>
    /// Handles report generation request.
    /// </summary>
    private static async Task<IResult> GenerateReportAsync(
        [FromBody] GenerateReportRequest request,
        [FromServices] ICommandHandler<GenerateReportCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new GenerateReportCommand(
            request.Type,
            request.StartPeriod,
            request.EndPeriod);

        var reportId = await handler.HandleAsync(command, cancellationToken);

        return Results.Accepted($"/reports/{reportId}/status", reportId);
    }

    /// <summary>
    /// Retrieves a report file by its ID.
    /// </summary>
    private static async Task<IResult> GetReportAsync(
        [FromRoute] Guid reportId,
        [FromServices] IQueryHandler<GetReportQuery, Stream> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetReportQuery(reportId);
        var reportStream = await handler.HandleAsync(query, cancellationToken);

        return Results.File(reportStream, "application/pdf");
    }
}
