using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Hosts.Api.Features.RateLimiting;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Users.Queries.GetCurrentUser;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Hosts.Api.Endpoints;

/// <summary>
/// Maps endpoints related to user operations.
/// </summary>
public static class UsersEndpoints
{
    /// <summary>
    /// Registers all budgets-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/users")
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitPolicy.Global)
            .WithTags("Users")
            .WithOpenApi();

        endpoints.MapGet("/me", GetCurrentUserAsync)
            .RequireRateLimiting(RateLimitPolicy.ReadOperations)
            .RequireRateLimiting(RateLimitPolicy.IpAddress)
            .WithName("GetCurrentUser")
            .WithSummary("Retrieves the current user.")
            .WithDescription("Returns the details of the currently authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }

    /// <summary>
    /// Retrieves the current user details.
    /// </summary>
    private static async Task<IResult> GetCurrentUserAsync(
        [FromServices] IQueryHandler<GetCurrentUserQuery, UserResponse> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery();
        var user = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(user);
    }
}