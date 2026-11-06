using System.Globalization;

namespace Veygo.Platform.Shared.Interfaces.Rest.Transform;

/// <summary>
///     Converts dates to and from the ISO <c>yyyy-MM-dd</c> strings used by the REST API.
/// </summary>
public static class DateValueAssembler
{
    private const string IsoDateFormat = "yyyy-MM-dd";

    /// <summary>Converts a date to <c>yyyy-MM-dd</c>.</summary>
    public static string ToValue(DateOnly date) => date.ToString(IsoDateFormat, CultureInfo.InvariantCulture);

    /// <summary>Converts an optional date to <c>yyyy-MM-dd</c> (empty string when null).</summary>
    public static string ToValue(DateOnly? date) => date.HasValue ? ToValue(date.Value) : string.Empty;

    /// <summary>
    ///     Parses a <c>yyyy-MM-dd</c> date (a full ISO date-time is also accepted; only its date part is used).
    /// </summary>
    /// <exception cref="ArgumentException">When the value is not a valid date.</exception>
    public static DateOnly ToDate(string? value, string fieldName)
    {
        var text = (value ?? string.Empty).Trim();
        if (text.Length >= IsoDateFormat.Length &&
            DateOnly.TryParseExact(text[..IsoDateFormat.Length], IsoDateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date))
            return date;
        throw new ArgumentException($"The {fieldName} must be a valid date (yyyy-MM-dd): '{value}'.", fieldName);
    }

    /// <summary>Parses an optional date (null when empty).</summary>
    /// <exception cref="ArgumentException">When the value is not empty and not a valid date.</exception>
    public static DateOnly? ToOptionalDate(string? value, string fieldName) =>
        string.IsNullOrWhiteSpace(value) ? null : ToDate(value, fieldName);
}
