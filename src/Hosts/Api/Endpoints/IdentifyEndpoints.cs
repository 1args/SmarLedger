using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Hosts.Api.Features.RateLimiting;
using SmartLedger.Hosts.Api.Helpers;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Login;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Logout;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Register;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.GetSessions;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken;
using SmartLedger.Modules.Security.Contracts.Requests.Identify;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;
using LoginRequest = SmartLedger.Modules.Security.Contracts.Requests.Identify.LoginRequest;

namespace SmartLedger.Hosts.Api.Endpoints;

/// <summary>
/// Maps endpoints related to identity operations.
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
            .RequireRateLimiting(RateLimitPolicy.Authentication)
            .RequireRateLimiting(RateLimitPolicy.IpAddress)
            .WithTags("Identify")
            .WithOpenApi();

        endpoints.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("Registers a new user.")
            .WithDescription("Creates a new user account with the provided username, email, first name, last name, and password.")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

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

        endpoints.MapPost("/logout", LogoutAsync)
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitPolicy.WriteOperations)
            .WithName("Logout")
            .WithSummary("Logs out the current user.")
            .WithDescription("Ends the current user's session and invalidates active tokens.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapGet("/me/sessions", GetUserSessionsAsync)
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitPolicy.ReadOperations)
            .WithName("GetUserSessions")
            .WithSummary("Retrieves the current user's active sessions.")
            .WithDescription("Returns a list of all active sessions associated with the currently authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }

    /// <summary>
    /// Registers a new user with the provided details.
    /// </summary>
    private static async Task<IResult> RegisterAsync(
        [FromBody] RegistrationRequest request,
        [FromServices] ICommandHandler<RegisterCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        await handler.HandleAsync(command, cancellationToken);

        return Results.Created();
    }

    /// <summary>
    /// Authenticates a user using provided credentials.
    /// </summary>
    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        [FromServices] ICommandHandler<LoginCommand, LoginResponse> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Username, request.Password);
        var response = await handler.HandleAsync(command, cancellationToken);

        CookieHelper.SetAccessTokenCookie(response.AccessToken, httpContext.Response.Cookies);
        CookieHelper.SetRefreshTokenCookie(response.RefreshToken, httpContext.Response.Cookies);

        return Results.Ok(response);
    }

    /// <summary>
    /// Refreshes access tokens using a valid refresh token.
    /// </summary>
    private static async Task<IResult> RefreshTokenAsync(
        [FromQuery] string refreshToken,
        [FromServices] IQueryHandler<RefreshTokenQuery, LoginResponse> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new RefreshTokenQuery(refreshToken);
        var response = await handler.HandleAsync(query, cancellationToken);

        CookieHelper.SetAccessTokenCookie(response.AccessToken, httpContext.Response.Cookies);
        CookieHelper.SetRefreshTokenCookie(response.RefreshToken, httpContext.Response.Cookies);

        return Results.Ok(response);
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    private static async Task<IResult> LogoutAsync(
        [FromServices] ICommandHandler<LogoutCommand> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new LogoutCommand(), cancellationToken);

        CookieHelper.ClearCookies(CookieHelper.AccessTokenCookieName, httpContext.Response.Cookies);
        CookieHelper.ClearCookies(CookieHelper.RefreshTokenCookieName, httpContext.Response.Cookies);

        return Results.Ok();
    }

    /// <summary>
    /// Retrieves all active sessions for the current user.
    /// </summary>
    private static async Task<IResult> GetUserSessionsAsync(
        [FromServices] IQueryHandler<GetSessionsQuery, IReadOnlyCollection<UserSessionResponse>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetSessionsQuery();
        var response = await handler.HandleAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}