namespace Veygo.Platform.Fleet.Domain.Model.ValueObjects;

/// <summary>
///     Catalog of the features a vehicle can offer (values shared with the web application).
/// </summary>
public static class VehicleFeatures
{
    /// <summary>
    ///     The supported feature values.
    /// </summary>
    public static readonly IReadOnlySet<string> Supported = new HashSet<string>
    {
        "air-conditioning", "bluetooth", "usb", "apple-carplay", "android-auto", "rear-camera",
        "gps", "sunroof", "child-seat", "cruise-control", "parking-sensor"
    };

    /// <summary>
    ///     Normalizes a list of features: trims, lower-cases, removes duplicates and validates each value.
    /// </summary>
    /// <param name="features">The features to normalize.</param>
    /// <returns>The normalized list.</returns>
    /// <exception cref="ArgumentException">When a feature is not supported.</exception>
    public static List<string> Normalize(IEnumerable<string>? features)
    {
        var normalized = (features ?? [])
            .Select(feature => feature.Trim().ToLowerInvariant())
            .Where(feature => feature.Length > 0)
            .Distinct()
            .ToList();

        var unsupported = normalized.FirstOrDefault(feature => !Supported.Contains(feature));
        if (unsupported is not null)
            throw new ArgumentException($"Unsupported vehicle feature: '{unsupported}'.", nameof(features));

        return normalized;
    }
}
