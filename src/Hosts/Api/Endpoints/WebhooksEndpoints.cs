using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.Handlers.Commands.CreateWebhookSubscription;
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
        var endpoints = app.MapGroup("/webhooks");

        endpoints.MapPost("/", CreateWebhookAsync);

        return app;
    }

    /// <summary>
    /// Creates a new webhook subscription.
    /// </summary>
    private static async Task<IResult> CreateWebhookAsync(
        [FromBody] CreateWebhookRequest request,
        [FromServices] ICommandHandler<CreateWebhookSubscriptionCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateWebhookSubscriptionCommand(
            request.EventType, request.WebhookUrl);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Created();
    }
}

