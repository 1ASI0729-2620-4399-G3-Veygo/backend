using Veygo.Platform.Booking.Interfaces.ACL;
using Veygo.Platform.Reputation.Domain.Model.ValueObjects;

namespace Veygo.Platform.Reputation.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service that queries the Booking context through its anti-corruption layer.
/// </summary>
public class ExternalBookingService(IBookingContextFacade bookingContextFacade)
{
    /// <summary>
    ///     Gets the summary of a booking, or null when it does not exist.
    /// </summary>
    public async Task<BookingSummary?> FetchBookingSummaryAsync(int bookingId,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookingContextFacade.FetchBookingSummaryAsync(bookingId, cancellationToken);
        return booking is null
            ? null
            : new BookingSummary(bookingId, booking.Value.VehicleId, booking.Value.OwnerId, booking.Value.RenterId,
                booking.Value.IsFinished);
    }
}
