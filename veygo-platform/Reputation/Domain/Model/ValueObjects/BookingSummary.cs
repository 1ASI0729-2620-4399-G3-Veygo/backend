namespace Veygo.Platform.Reputation.Domain.Model.ValueObjects;

/// <summary>
///     The data of a booking that the Reputation context needs (obtained from Booking through its ACL).
/// </summary>
public record BookingSummary(int BookingId, int VehicleId, int OwnerId, int RenterId, bool IsFinished);
