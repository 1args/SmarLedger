using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Host.Public.Helpers;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Login;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Logout;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Register;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.GetSessions;
using SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken;
using SmartLedger.Modules.Security.Contracts.Requests.Identify;
using LoginRequest = SmartLedger.Modules.Security.Contracts.Requests.Identify.LoginRequest;

namespace SmartLedger.Host.Public.Endpoints;

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
            .WithTags("Identify")
            .WithOpenApi();

        endpoints.MapPost("/register", Register)
            .WithName("Register")
            .WithSummary("Registers a new user.")
            .WithDescription("Creates a new user account with the provided username, email, first name, last name, and password.")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        endpoints.MapPost("/login", Login)
            .WithName("Login")
            .WithSummary("Authenticates a user.")
            .WithDescription("Authenticates a user using username and password, returning access and refresh tokens.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapPost("/refresh-token", RefreshToken)
            .WithName("RefreshToken")
            .WithSummary("Refreshes the access token.")
            .WithDescription("Uses a valid refresh token to generate a new access token.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapPost("/logout", Logout)
            .RequireAuthorization()
            .WithName("Logout")
            .WithSummary("Logs out the current user.")
            .WithDescription("Ends the current user's session and invalidates active tokens.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        endpoints.MapGet("/me/sessions", GetUserSessions)
            .RequireAuthorization()
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
    private static async Task<IResult> Register(
        [FromBody] RegistrationRequest request,
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);
        await bus.SendAsync(command, cancellationToken);

        return Results.Created();
    }

    /// <summary>
    /// Authenticates a user using provided credentials.
    /// </summary>
    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] IMessageBus bus,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Username, request.Password);
        var response = await bus.SendAsync(command, cancellationToken);

        CookieHelper.SetAccessTokenCookie(response.AccessToken, httpContext.Response.Cookies);
        CookieHelper.SetRefreshTokenCookie(response.RefreshToken, httpContext.Response.Cookies);

        return Results.Ok(response);
    }

    /// <summary>
    /// Refreshes access tokens using a valid refresh token.
    /// </summary>
    private static async Task<IResult> RefreshToken(
        [FromQuery] [BindRequired] string refreshToken,
        [FromServices] IMessageBus bus,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new RefreshTokenQuery(refreshToken);
        var response = await bus.QueryAsync(query, cancellationToken);

        CookieHelper.SetAccessTokenCookie(response.AccessToken, httpContext.Response.Cookies);
        CookieHelper.SetRefreshTokenCookie(response.RefreshToken, httpContext.Response.Cookies);

        return Results.Ok(response);
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    private static async Task<IResult> Logout(
        [FromServices] IMessageBus bus,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new LogoutCommand();
        await bus.SendAsync(command, cancellationToken);

        CookieHelper.ClearCookies(CookieHelper.AccessTokenCookieName, httpContext.Response.Cookies);
        CookieHelper.ClearCookies(CookieHelper.RefreshTokenCookieName, httpContext.Response.Cookies);

        return Results.Ok();
    }

    /// <summary>
    /// Retrieves all active sessions for the current user.
    /// </summary>
    private static async Task<IResult> GetUserSessions(
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var query = new GetSessionsQuery();
        var response = await bus.QueryAsync(query, cancellationToken);

        return Results.Ok(response);
    }
}