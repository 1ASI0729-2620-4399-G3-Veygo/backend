using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Shared.Domain.Model.Errors;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Shared.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an RFC 9457 Problem Details action result from a domain <see cref="Error" />.
/// </summary>
public static class ActionResultFromErrorAssembler
{
    /// <summary>
    ///     Maps an error type to its HTTP status code.
    /// </summary>
    /// <param name="type">The error type.</param>
    /// <returns>The HTTP status code.</returns>
    public static int ToStatusCode(EErrorType type) => type switch
    {
        EErrorType.Validation => StatusCodes.Status400BadRequest,
        EErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        EErrorType.Forbidden => StatusCodes.Status403Forbidden,
        EErrorType.NotFound => StatusCodes.Status404NotFound,
        EErrorType.Conflict => StatusCodes.Status409Conflict,
        EErrorType.Failure => StatusCodes.Status422UnprocessableEntity,
        _ => StatusCodes.Status500InternalServerError
    };

    /// <summary>
    ///     Builds the Problem Details response for the given error.
    /// </summary>
    /// <remarks>
    ///     The title is localized from <c>ErrorType.{Type}</c> and the detail from the error <c>Code</c>.
    ///     When a code has no localized text, the default message of the error is used.
    ///     The error code is also returned in the <c>code</c> extension so clients can react to it.
    /// </remarks>
    /// <param name="error">The domain error.</param>
    /// <param name="controller">The controller producing the response.</param>
    /// <param name="localizer">The localizer for error messages.</param>
    /// <returns>The action result.</returns>
    public static ActionResult ToActionResultFromError(
        Error error,
        ControllerBase controller,
        IStringLocalizer<ErrorMessages> localizer)
    {
        var statusCode = ToStatusCode(error.Type);
        var localizedDetail = localizer[error.Code];

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = localizer[$"ErrorType.{error.Type}"].Value,
            Detail = localizedDetail.ResourceNotFound ? error.Message : localizedDetail.Value,
            Instance = controller.HttpContext.Request.Path
        };
        problemDetails.Extensions["code"] = error.Code;

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}
