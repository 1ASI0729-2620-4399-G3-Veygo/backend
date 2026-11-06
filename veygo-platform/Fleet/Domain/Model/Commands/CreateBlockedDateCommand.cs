namespace Veygo.Platform.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to block a day in the availability calendar of a vehicle.
/// </summary>
public record CreateBlockedDateCommand(int VehicleId, DateOnly Date);
