namespace Veygo.Platform.Booking.Interfaces.ACL;

/// <summary>
///     Anti-corruption layer facade that the other bounded contexts use to query the Booking context.
/// </summary>
public interface IBookingContextFacade
{
    /// <summary>
    ///     Gets the participants of a booking and whether the rental already finished, or null when the booking does
    ///     not exist.
    /// </summary>
    Task<(int VehicleId, int OwnerId, int RenterId, bool IsFinished)?> FetchBookingSummaryAsync(int bookingId,
        CancellationToken cancellationToken = default);
}