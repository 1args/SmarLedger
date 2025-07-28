using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Domain.Exceptions;

namespace SmartLedger.Hosts.Api.Features.ExceptionHandler;

/// <summary>
/// Represents a global exception handler that catches and processes exceptions
/// thrown during the request lifecycle, and returns standardized Problem Details responses.
/// </summary>
/// <param name="logger">Logger used to record exception details.</param>
/// <param name="detailsService">Service used to write Problem Details responses.</param>
public sealed class GlobalExceptionHandler(
    IHostEnvironment environment,
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService detailsService) : IExceptionHandler
{
    /// <inheritdoc/>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = GetStatusCode(httpContext, exception);
        var problemDetails = CreateProblemDetails(httpContext, exception, statusCode);

        logger.LogError(
            "Exception occurred while processing the HTTP {Method} {Path} with status code {StatusCode} and message: {Message}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            (int)statusCode,
            exception.Message);

        httpContext.Response.StatusCode = (int)statusCode;

        return await detailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
    }

    /// <summary>
    /// Creates a ProblemDetails object based on the exception.
    /// </summary>
    private ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        Exception exception,
        HttpStatusCode statusCode)
    {
        string? title, detail;

        if (environment.IsDevelopment())
        {
            title = $"$Error '{exception.GetType().FullName}' occurred with the following context: {exception.Message}";
            detail = exception.ToString();
        }
        else
        {
            (title, detail) = exception switch
            {
                ValidationException => ("Validation error", exception.Message),
                NotFoundException => ("Not Found", exception.Message),
                ConflictException => ("Conflict", exception.Message),
                DomainValidationException => ("Bad Request", exception.Message),
                DomainException => ("Bad Request", exception.Message),
                OperationCanceledException => ("Request Timeout", "The request was canceled due to a timeout."),
                _ => ("Internal Server Error", "Something went wrong while executing the request. Please try again later.")
            };
        }

        return new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Type = GetErrorType((int)statusCode),
            Detail = detail
        };
    }

    /// <summary>
    /// Determines the appropriate HTTP status code based on the exception type and request path.
    /// </summary>
    private static HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {
        if (httpContext.Request.Path.HasValue && httpContext.Request.Path.Value.Contains("token/refresh"))
        {
            return HttpStatusCode.Unauthorized;
        }

        return exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            ConflictException => HttpStatusCode.Conflict,
            DomainValidationException => HttpStatusCode.BadRequest,
            DomainException => HttpStatusCode.BadRequest,
            OperationCanceledException => HttpStatusCode.RequestTimeout,
            _ => HttpStatusCode.InternalServerError
        };
    }

    /// <summary>
    /// Gets the error type URL based on the status code.
    /// </summary>
    private static string GetErrorType(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
        StatusCodes.Status404NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
        StatusCodes.Status409Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
        StatusCodes.Status408RequestTimeout => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.7",
        _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
    };
}