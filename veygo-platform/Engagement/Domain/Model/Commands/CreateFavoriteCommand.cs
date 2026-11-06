namespace Veygo.Platform.Engagement.Domain.Model.Commands;

/// <summary>
///     Command to save a vehicle in the favorites of a renter.
/// </summary>
public record CreateFavoriteCommand(int RenterId, int VehicleId);
