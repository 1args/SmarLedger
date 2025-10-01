using SmartLedger.Common.Contracts.Authorization;
using System.Security.Claims;

namespace SmartLedger.Hosts.Api.Middlewares;

/// <summary>
/// Middleware for handling authorization by extracting user ID from the request context.
/// </summary>
public class AuthorizationMiddleware(RequestDelegate next)
{
    /// <summary>Path to the register endpoint.</summary>
    private static readonly string RegisterPath = "/identify/register";

    /// <summary>Path to the hangfire endpoint.</summary>
    private static readonly string HangfirePath = "/hangfire";

    /// <summary>Path to the login endpoint.</summary>
    private static readonly string LoginPath = "/identify/login";

    /// <summary>Path to the refresh token endpoint.</summary>
    private static readonly string RefreshTokenPath = "/identify/refresh-token";

    /// <summary>List of allowed paths that do not require authorization.</summary>
    private static readonly string[] AllowedPaths =
    [
        HangfirePath,
        RegisterPath,
        LoginPath,
        RefreshTokenPath
    ];

    /// <summary>
    /// Invokes the middleware to handle authorization by checking the user ID in the request context.
    /// </summary>
    /// <param name="context">Http Context.</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (IsPathAllowed(httpContext.Request.Path))
        {
            await next(httpContext);
            return;
        }

        var userId = FindUserId(httpContext.User);

        if (string.IsNullOrEmpty(userId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await httpContext.Response.WriteAsync("Missing 'sub' claim in token.");

            return;
        }

        if (!Guid.TryParse(userId, out var passedUserId))
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await httpContext.Response.WriteAsync($"Invalid user ID format: {userId}");

            return;
        }

        var authorizationData = httpContext.RequestServices.GetRequiredService<Lazy<IAuthorizationData>>();
        authorizationData.Value.UserId = passedUserId;

        await next(httpContext);
    }

    /// <summary>
    /// Checks if the request path is allowed and does not require authorization.
    /// </summary>
    private static bool IsPathAllowed(PathString path) =>
        AllowedPaths.Any(allowedPath => path.StartsWithSegments(allowedPath));

    /// <summary>
    /// Finds the user ID from the claims principal.
    /// </summary>
    private static string FindUserId(ClaimsPrincipal user)
    {
        var userId = user.FindFirst("sub")?.Value;

        if (!string.IsNullOrWhiteSpace(userId))
        {
            return userId;
        }

        foreach (var claim in user.Claims)
        {
            if (claim.Type == ClaimTypes.NameIdentifier)
            {
                return claim.Value;
            }
            if (claim.Type.EndsWith("/sub"))
            {
                return claim.Value;
            }
        }

        return string.Empty;
    }
}