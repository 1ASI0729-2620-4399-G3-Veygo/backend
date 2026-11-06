using System.Text.RegularExpressions;

namespace Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Extension methods for <see cref="string" /> used by the ASP.NET Core configuration.
/// </summary>
public static partial class StringExtensions
{
    /// <summary>
    ///     Converts the text to kebab case (e.g. <c>BlockedDates</c> → <c>blocked-dates</c>).
    /// </summary>
    /// <param name="text">The string to convert.</param>
    /// <returns>The kebab case string.</returns>
    public static string ToKebabCase(this string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        return KebabCaseRegex().Replace(text, "-$1")
            .Trim()
            .ToLower();
    }

    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled)]
    private static partial Regex KebabCaseRegex();
}
