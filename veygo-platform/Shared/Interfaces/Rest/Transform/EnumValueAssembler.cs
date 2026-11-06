using Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

namespace Veygo.Platform.Shared.Interfaces.Rest.Transform;

/// <summary>
///     Converts domain enums to and from the kebab-case values used by the REST API
///     (e.g. <c>AirConditioning</c> ↔ <c>"air-conditioning"</c>, <c>Suv</c> ↔ <c>"suv"</c>).
/// </summary>
public static class EnumValueAssembler
{
    /// <summary>
    ///     Converts an enum value to its REST representation.
    /// </summary>
    public static string ToValue<TEnum>(TEnum value) where TEnum : struct, Enum => value.ToString().ToKebabCase();

    /// <summary>
    ///     Converts an optional enum value to its REST representation (empty string when null).
    /// </summary>
    public static string ToValue<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        value.HasValue ? ToValue(value.Value) : string.Empty;

    /// <summary>
    ///     Parses a REST value into an enum.
    /// </summary>
    /// <exception cref="ArgumentException">When the value is not supported.</exception>
    public static TEnum ToEnum<TEnum>(string? value, string fieldName) where TEnum : struct, Enum
    {
        var normalized = (value ?? string.Empty).Replace("-", string.Empty).Trim();
        if (normalized.Length == 0 || !Enum.TryParse<TEnum>(normalized, true, out var result) ||
            !Enum.IsDefined(result) || int.TryParse(normalized, out _))
            throw new ArgumentException($"Unsupported {fieldName}: '{value}'.", fieldName);
        return result;
    }

    /// <summary>
    ///     Parses an optional REST value into an enum (null when empty).
    /// </summary>
    /// <exception cref="ArgumentException">When the value is not empty and not supported.</exception>
    public static TEnum? ToOptionalEnum<TEnum>(string? value, string fieldName) where TEnum : struct, Enum =>
        string.IsNullOrWhiteSpace(value) ? null : ToEnum<TEnum>(value, fieldName);
}
