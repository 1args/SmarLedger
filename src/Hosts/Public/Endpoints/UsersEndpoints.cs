using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Users.Queries.GetCurrentUser;

namespace SmartLedger.Host.Public.Endpoints;

/// <summary>
/// Maps endpoints related to user operations.
/// </summary>
public static class UsersEndpoints
{
    /// <summary>
    /// Registers all users-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/users")
            .RequireAuthorization()
            .WithTags("Users")
            .WithOpenApi();

        endpoints.MapGet("/me", GetCurrentUser)
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
    private static async Task<IResult> GetCurrentUser(
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery();
        var user = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(user);
    }
}