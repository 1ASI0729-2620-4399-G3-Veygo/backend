namespace Veygo.Platform.Reputation.Interfaces.Rest.Resources;

/// <summary>
///     Review of a finished rental.
/// </summary>
public record ReviewResource(
    int Id,
    int BookingId,
    int VehicleId,
    int OwnerId,
    int RenterId,
    int Rating,
    string Comment,
    DateTimeOffset? CreatedAt);
