namespace Veygo.Platform.Shared.Domain.Model.ValueObjects;

/// <summary>
///     Provides the current date in the business time zone of the platform (Lima, Peru — UTC-05:00).
/// </summary>
/// <remarks>
///     Servers and containers usually run in UTC; after 19:00 in Lima the UTC date is already "tomorrow".
///     Rules such as "a rental must start today or later" must use the business date instead.
/// </remarks>
public static class BusinessCalendar
{
    private static readonly TimeZoneInfo BusinessTimeZone = ResolveBusinessTimeZone();

    /// <summary>
    ///     Gets the current date in Lima.
    /// </summary>
    public static DateOnly Today => DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, BusinessTimeZone).DateTime);

    private static TimeZoneInfo ResolveBusinessTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Lima");
        }
        catch (Exception exception) when (exception is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            return TimeZoneInfo.CreateCustomTimeZone("Lima", TimeSpan.FromHours(-5), "Lima", "Lima");
        }
    }
}
