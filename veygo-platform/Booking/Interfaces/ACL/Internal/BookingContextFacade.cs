using Veygo.Platform.Booking.Application.QueryServices;
using Veygo.Platform.Booking.Domain.Model.Queries;
using Veygo.Platform.Shared.Domain.Model.ValueObjects;

namespace Veygo.Platform.Booking.Interfaces.ACL.Internal;

/// <summary>
///     Implementation of <see cref="IBookingContextFacade" />.
/// </summary>
public class BookingContextFacade(IBookingQueryService bookingQueryService) : IBookingContextFacade
{
    /// <inheritdoc />
    public async Task<(int VehicleId, int OwnerId, int RenterId, bool IsFinished)?> FetchBookingSummaryAsync(
        int bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await bookingQueryService.Handle(new GetBookingByIdQuery(bookingId), cancellationToken);
        if (booking is null) return null;
        return (booking.VehicleId, booking.OwnerId, booking.RenterId, booking.IsFinishedOn(BusinessCalendar.Today));
    }
}