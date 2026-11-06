namespace Veygo.Platform.Fleet.Domain.Model.ValueObjects;

/// <summary>
///     Pick-up location of a vehicle.
/// </summary>
/// <param name="Address">Street address.</param>
/// <param name="District">District of Lima (e.g. San Isidro).</param>
/// <param name="City">City.</param>
/// <param name="Latitude">Latitude in decimal degrees (optional).</param>
/// <param name="Longitude">Longitude in decimal degrees (optional).</param>
public record VehicleLocation(string Address, string District, string City, double? Latitude, double? Longitude)
{
    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    public VehicleLocation() : this(string.Empty, string.Empty, "Lima", null, null)
    {
    }

    /// <summary>
    ///     Gets a value indicating whether the location has map coordinates.
    /// </summary>
    public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;
}
