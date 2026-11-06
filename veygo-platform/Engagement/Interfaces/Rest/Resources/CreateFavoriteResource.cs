using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Engagement.Interfaces.Rest.Resources;

/// <summary>
///     Data required to save a vehicle in the favorites of a renter.
/// </summary>
public record CreateFavoriteResource([Range(1, int.MaxValue)] int RenterId, [Range(1, int.MaxValue)] int VehicleId);
