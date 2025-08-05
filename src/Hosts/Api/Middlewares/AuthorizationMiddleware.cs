using SmartLedger.Common.Contracts.Authorization;

namespace SmartLedger.Hosts.Api.Middlewares;

public class AuthorizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IAuthorizationData authorizationData)
    {
        if (!context.User.Identity!.IsAuthenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("User is not authenticated.");
            return;
        }

        var userId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var passedUserId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Invalid or missing user ID in token.");
            return;
        }

        authorizationData.UserId = passedUserId;
    }
}
