using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.Handlers.Contexts.Webhooks.Commands.CreateWebhook;
using SmartLedger.Modules.Webhooks.Contracts.Requests;

namespace SmartLedger.Hosts.Api.Endpoints;

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

        endpoints.MapPost("/", CreateWebhookAsync)
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
    private static async Task<IResult> CreateWebhookAsync(
        [FromBody] CreateWebhookRequest request,
        [FromServices] ICommandHandler<CreateWebhookCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateWebhookCommand(
            request.EventType, request.CallbackUrl);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Created();
    }
}