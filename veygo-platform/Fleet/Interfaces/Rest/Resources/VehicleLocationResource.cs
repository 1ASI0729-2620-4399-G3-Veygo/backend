namespace Veygo.Platform.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Pick-up location of a vehicle.
/// </summary>
public record VehicleLocationResource(
    string? Address,
    string? District,
    string? City,
    double? Latitude,
    double? Longitude);
