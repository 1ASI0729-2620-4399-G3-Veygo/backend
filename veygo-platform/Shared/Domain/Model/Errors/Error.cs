namespace Veygo.Platform.Shared.Domain.Model.Errors;

/// <summary>
///     Classification of a domain error. Each type maps to a standard HTTP status code in the REST layer.
/// </summary>
public enum EErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Unexpected = 6
}

/// <summary>
///     Represents a domain error.
/// </summary>
/// <param name="Code">
///     The unique error code (e.g. <c>Fleet.VehicleNotFound</c>). It is also the key of the localized message in
///     <c>ErrorMessages.resx</c>.
/// </param>
/// <param name="Message">The default (English) error message.</param>
/// <param name="Type">The error classification type.</param>
public record Error(string Code, string Message, EErrorType Type = EErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string code, string message) => new(code, message, EErrorType.NotFound);
    public static Error Conflict(string code, string message) => new(code, message, EErrorType.Conflict);
    public static Error Validation(string code, string message) => new(code, message, EErrorType.Validation);
    public static Error Failure(string code, string message) => new(code, message, EErrorType.Failure);
    public static Error Unauthorized(string code, string message) => new(code, message, EErrorType.Unauthorized);
    public static Error Forbidden(string code, string message) => new(code, message, EErrorType.Forbidden);
    public static Error Unexpected(string code, string message) => new(code, message, EErrorType.Unexpected);
}
