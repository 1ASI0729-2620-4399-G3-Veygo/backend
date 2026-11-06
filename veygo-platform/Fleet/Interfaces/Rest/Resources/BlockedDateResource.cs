namespace Veygo.Platform.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Day in which a vehicle is not available (<c>yyyy-MM-dd</c>).
/// </summary>
public record BlockedDateResource(int Id, int VehicleId, string Date);
