using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Property builder extensions with the value conversions shared by all bounded contexts.
/// </summary>
public static class PropertyBuilderExtensions
{
    private static readonly ValueConverter<DateOnly, DateTime> DateOnlyConverter =
        new(date => date.ToDateTime(TimeOnly.MinValue), dateTime => DateOnly.FromDateTime(dateTime));

    /// <summary>
    ///     Stores a <see cref="DateOnly" /> as a MySQL <c>date</c> column.
    /// </summary>
    public static PropertyBuilder<DateOnly> HasDateConversion(this PropertyBuilder<DateOnly> builder) =>
        builder.HasConversion(DateOnlyConverter).HasColumnType("date");

    /// <summary>
    ///     Stores an optional <see cref="DateOnly" /> as a nullable MySQL <c>date</c> column.
    /// </summary>
    public static PropertyBuilder<DateOnly?> HasDateConversion(this PropertyBuilder<DateOnly?> builder) =>
        builder.HasConversion(DateOnlyConverter).HasColumnType("date");

    /// <summary>
    ///     Stores a list of strings as a JSON text column (e.g. <c>["usb","gps"]</c>).
    /// </summary>
    public static PropertyBuilder<List<string>> HasJsonConversion(this PropertyBuilder<List<string>> builder)
    {
        var comparer = new ValueComparer<List<string>>(
            (left, right) => (left ?? new List<string>()).SequenceEqual(right ?? new List<string>()),
            list => list.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
            list => list.ToList());

        return builder.HasConversion(
            list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null) ?? new List<string>(),
            comparer);
    }
}
