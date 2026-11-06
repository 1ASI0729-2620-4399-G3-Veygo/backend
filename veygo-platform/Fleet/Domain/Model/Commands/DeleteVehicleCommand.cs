namespace Veygo.Platform.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to remove a vehicle from the fleet.
/// </summary>
public record DeleteVehicleCommand(int VehicleId);
