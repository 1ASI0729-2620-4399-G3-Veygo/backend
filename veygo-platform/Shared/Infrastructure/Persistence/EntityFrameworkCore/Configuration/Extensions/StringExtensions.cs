using Humanizer;

namespace Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     String extensions for the database context.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Converts the text to snake case (e.g. <c>PricePerDay</c> → <c>price_per_day</c>).
    /// </summary>
    /// <param name="text">The text to convert.</param>
    /// <returns>The snake cased text.</returns>
    public static string ToSnakeCase(this string text)
    {
        return new string(Convert(text.GetEnumerator()).ToArray());

        static IEnumerable<char> Convert(CharEnumerator e)
        {
            if (!e.MoveNext()) yield break;

            yield return char.ToLower(e.Current);

            while (e.MoveNext())
                if (char.IsUpper(e.Current))
                {
                    yield return '_';
                    yield return char.ToLower(e.Current);
                }
                else
                {
                    yield return e.Current;
                }
        }
    }

    /// <summary>
    ///     Converts the text to plural.
    /// </summary>
    /// <param name="text">The text to convert.</param>
    /// <returns>The pluralized text.</returns>
    public static string ToPlural(this string text)
    {
        return text.Pluralize(false);
    }
}
