using Microsoft.AspNetCore.Http.Features;
using SmartLedger.Common.Hosts.Features.Abstractions;

namespace SmartLedger.Hosts.Api.Features.ExceptionHandler;

/// <summary>
/// Feature for configuring a global exception handler.
/// </summary>
internal class GlobalExceptionHandlerFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}
