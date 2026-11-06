namespace Veygo.Platform.Engagement.Interfaces.Rest.Resources;

/// <summary>
///     Favorite vehicle of a renter.
/// </summary>
public record FavoriteResource(int Id, int RenterId, int VehicleId, DateTimeOffset? CreatedAt);
