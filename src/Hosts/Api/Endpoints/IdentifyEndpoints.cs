using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Login;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Logout;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.ResetPassword;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.GetSessions;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken;
using SmartLedger.Modules.Security.Contracts.Requests.Identify;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Hosts.Api.Endpoints;

/// <summary>
/// Maps endpoints related to identity operations such as login, logout, token refresh, and session management.
/// </summary>
public static class IdentifyEndpoints
{
    /// <summary>
    /// Registers all identify-related routes.
    /// </summary>
    /// <param name="app">Application's endpoint route builder.</param>
    /// <returns>Modified <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapIdentifyEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/identify")
            .WithTags("Identify")
            .WithOpenApi();

        endpoints.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Authenticates a user.")
            .WithDescription("Authenticates a user using username and password, returning access and refresh tokens.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapPost("/refresh-token", RefreshTokenAsync)
            .WithName("RefreshToken")
            .WithSummary("Refreshes the access token.")
            .WithDescription("Uses a valid refresh token to generate a new access token.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapPost("/reset-password", ResetPasswordAsync)
            .WithName("ResetPassword")
            .WithSummary("Resets the user's password.")
            .WithDescription("Allows the user to reset their password by providing the current and new passwords.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapPost("/logout", LogoutAsync)
            .WithName("Logout")
            .WithSummary("Logs out the current user.")
            .WithDescription("Ends the current user's session and invalidates active tokens.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapGet("/me/sessions", GetUserSessionsAsync)
            .WithName("GetUserSessions")
            .WithSummary("Retrieves the current user's active sessions.")
            .WithDescription("Returns a list of all active sessions associated with the currently authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }

    /// <summary>
    /// Authenticates a user using provided credentials.
    /// </summary>
    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        [FromServices] ICommandHandler<LoginCommand, LoginResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Username, request.Password);
        var response = await handler.HandleAsync(command, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Refreshes access tokens using a valid refresh token.
    /// </summary>
    private static async Task<IResult> RefreshTokenAsync(
        [FromBody] string refreshToken,
        [FromServices] IQueryHandler<RefreshTokenQuery, LoginResponse> handler,
        CancellationToken cancellationToken)
    {
        var query = new RefreshTokenQuery(refreshToken);
        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }

    /// <summary>
    /// Resets the current user's password.
    /// </summary>
    private static async Task<IResult> ResetPasswordAsync(
        [FromBody] ResetPasswordRequest request,
        [FromServices] ICommandHandler<ResetPasswordCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(request.CurrentPassword, request.NewPassword);
        await handler.HandleAsync(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    private static async Task<IResult> LogoutAsync(
        [FromServices] ICommandHandler<LogoutCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new LogoutCommand(), cancellationToken);

        return Results.SignOut();
    }

    /// <summary>
    /// Retrieves all active sessions for the current user.
    /// </summary>
    private static async Task<IResult> GetUserSessionsAsync(
        [FromServices] IQueryHandler<GetSessionsQuery, List<UserSessionResponse>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetSessionsQuery();
        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}