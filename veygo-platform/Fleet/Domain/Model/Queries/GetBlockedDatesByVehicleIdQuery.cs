namespace Veygo.Platform.Fleet.Domain.Model.Queries;

/// <summary>
///     Query to get the blocked days of a vehicle.
/// </summary>
public record GetBlockedDatesByVehicleIdQuery(int VehicleId);
