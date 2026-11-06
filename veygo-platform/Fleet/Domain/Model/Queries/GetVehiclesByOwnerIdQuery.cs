namespace Veygo.Platform.Fleet.Domain.Model.Queries;

/// <summary>
///     Query to get the vehicles of an owner.
/// </summary>
public record GetVehiclesByOwnerIdQuery(int OwnerId);
