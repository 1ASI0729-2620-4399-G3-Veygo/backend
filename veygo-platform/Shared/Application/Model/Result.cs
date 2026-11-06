using Veygo.Platform.Shared.Domain.Model.Errors;

namespace Veygo.Platform.Shared.Application.Model;

/// <summary>
///     Generic result returned by command services in the application layer.
/// </summary>
/// <remarks>
///     A result is either a success carrying a value, or a failure carrying a domain <see cref="Error" />.
///     Expected business failures (not found, conflict, unavailable...) are returned as results instead of
///     being thrown as exceptions.
/// </remarks>
/// <typeparam name="T">The type of the success value.</typeparam>
public class Result<T>
{
    private Result(bool isSuccess, T? value, Error error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>Gets a value indicating whether the operation succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>Gets a value indicating whether the operation failed.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>Gets the success value (only meaningful when <see cref="IsSuccess" /> is true).</summary>
    public T? Value { get; }

    /// <summary>Gets the error (<see cref="Error.None" /> on success).</summary>
    public Error Error { get; }

    /// <summary>Creates a successful result.</summary>
    public static Result<T> Success(T value) => new(true, value, Error.None);

    /// <summary>Creates a failed result.</summary>
    public static Result<T> Failure(Error error) => new(false, default, error);

    /// <summary>
    ///     Applies one of the two functions depending on the outcome.
    /// </summary>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(Error);
}
