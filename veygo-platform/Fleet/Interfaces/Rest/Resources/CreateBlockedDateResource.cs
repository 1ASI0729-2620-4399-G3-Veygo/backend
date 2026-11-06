using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Data required to block a day (<c>yyyy-MM-dd</c>) for a vehicle.
/// </summary>
public record CreateBlockedDateResource([Range(1, int.MaxValue)] int VehicleId, [Required] string Date);
