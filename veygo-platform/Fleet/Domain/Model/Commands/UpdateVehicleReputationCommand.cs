namespace Veygo.Platform.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to update the rating summary shown for a vehicle.
/// </summary>
public record UpdateVehicleReputationCommand(int VehicleId, double Rating, int ReviewsCount);
