using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.Handlers.Contexts.Webhooks.Commands.CreateWebhook;
using SmartLedger.Modules.Webhooks.Contracts.Requests;

namespace SmartLedger.Host.Public.Endpoints;

/// <summary>
/// Maps endpoints related to webhooks operations.
/// </summary>
public static class WebhooksEndpoints
{
    /// <summary>
    /// Registers all webhooks-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapWebhooksEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/webhooks")
            .RequireAuthorization()
            .WithTags("Webhooks")
            .WithOpenApi();

        endpoints.MapPost("/", CreateWebhook)
            .WithName("CreateWebhook")
            .WithSummary("Creates a new webhook subscription.")
            .WithDescription("Creates a new webhook subscription for the specified event type and callback URL.")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    /// <summary>
    /// Creates a new webhook subscription.
    /// </summary>
    private static async Task<IResult> CreateWebhook(
        [FromBody] CreateWebhookRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new CreateWebhookCommand(
            request.EventType, request.CallbackUrl);
        await bus.SendAsync(command, cancellationToken);

        return Results.Created();
    }
}