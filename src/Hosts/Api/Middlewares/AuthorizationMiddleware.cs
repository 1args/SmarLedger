using SmartLedger.Common.Contracts.Authorization;
using System.Security.Claims;

namespace SmartLedger.Hosts.Api.Middlewares;

public class AuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/identify/login"))
        {
            await next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/identify/refresh-token"))
        {
            await next(context);
            return;
        }

        Console.WriteLine("Claims present in token:");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }

        var userId = context.User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            userId = context.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        if (string.IsNullOrEmpty(userId))
        {
            userId = context.User.Claims
                .FirstOrDefault(c => c.Type.EndsWith("/sub"))?.Value;
        }

        if (string.IsNullOrEmpty(userId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Missing 'sub' claim in token.");
            return;
        }

        if (!Guid.TryParse(userId, out var passedUserId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync($"Invalid user ID format: {userId}");
            return;
        }

        var authorizationData = context.RequestServices.GetRequiredService<Lazy<IAuthorizationData>>();
        authorizationData.Value.UserId = passedUserId;

        await next(context);
    }
}
