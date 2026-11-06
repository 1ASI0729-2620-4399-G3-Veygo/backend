namespace Veygo.Platform.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to publish or unpublish a vehicle in the catalog.
/// </summary>
public record UpdateVehiclePublicationCommand(int VehicleId, bool Published);
