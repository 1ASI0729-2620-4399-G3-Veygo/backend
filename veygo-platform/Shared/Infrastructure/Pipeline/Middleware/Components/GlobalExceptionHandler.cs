using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MySql.Data.MySqlClient;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Global exception handler implementing ASP.NET Core <see cref="IExceptionHandler" />.
/// </summary>
/// <remarks>
///     Writes RFC 9457 Problem Details responses for exceptions that escape the controllers:
///     <list type="bullet">
///         <item><description><see cref="ArgumentException" /> (domain invariant violated) → 400 Bad Request.</description></item>
///         <item><description>Duplicate key in MySQL (error 1062) → 409 Conflict.</description></item>
///         <item><description>Request aborted by the client → 499 without body.</description></item>
///         <item><description>Anything else → 500 Internal Server Error.</description></item>
///     </list>
/// </remarks>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : IExceptionHandler
{
    private const int MySqlDuplicateEntryErrorNumber = 1062;
    private const int ClientClosedRequestStatusCode = 499;

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            logger.LogInformation("Request {Path} was cancelled by the client.", httpContext.Request.Path);
            httpContext.Response.StatusCode = ClientClosedRequestStatusCode;
            return true;
        }

        var (statusCode, titleKey, detail) = exception switch
        {
            ArgumentException argumentException =>
                (StatusCodes.Status400BadRequest, "ErrorType.Validation", argumentException.Message),
            DbUpdateException { InnerException: MySqlException { Number: MySqlDuplicateEntryErrorNumber } } =>
                (StatusCodes.Status409Conflict, "ErrorType.Conflict", errorLocalizer["DuplicateResource"].Value),
            _ => (StatusCodes.Status500InternalServerError, "ErrorType.Unexpected", errorLocalizer["GenericError"].Value)
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
        else
            logger.LogWarning("Request {Path} failed with {StatusCode}: {Message}", httpContext.Request.Path, statusCode,
                exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = errorLocalizer[titleKey].Value,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}
