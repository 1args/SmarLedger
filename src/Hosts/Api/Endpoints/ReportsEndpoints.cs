using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Hosts.Api.Features.RateLimiting;
using SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Commands.GenerateReport;
using SmartLedger.Modules.Reports.Applications.Handlers.Contexts.Reports.Queries.GetReport;
using SmartLedger.Modules.Reports.Contracts.Requests;

namespace SmartLedger.Hosts.Api.Endpoints;

public static class ReportsEndpoints
{
    public static IEndpointRouteBuilder MapReportsEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/reports")
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitPolicy.Global)
            .RequireRateLimiting(RateLimitPolicy.IpAddress);

        endpoints.MapPost("/generate", GenerateReportAsync);

        endpoints.MapGet("/get", GetReportAsync);

        return app;
    }

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

        return Results.Ok(reportId);
    }

    private static async Task<IResult> GetReportAsync(
        [FromQuery] string reportPath,
        [FromServices] IQueryHandler<GetReportQuery, Stream> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetReportQuery(reportPath);
        var reportStream = await handler.HandleAsync(query, cancellationToken);

        return Results.File(reportStream, "application/pdf");
    }
}
