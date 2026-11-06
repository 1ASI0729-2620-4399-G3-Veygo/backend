using Veygo.Platform.Booking.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Application.CommandServices;

/// <summary>
///     Command service for bookings.
/// </summary>
public interface IBookingCommandService
{
    /// <summary>
    ///     Requests a booking. Validates that the vehicle exists, is published, is not owned by the renter
    ///     and is available (no active booking overlaps and no night is blocked).
    /// </summary>
    Task<Result<BookingAggregate>> Handle(CreateBookingCommand command, CancellationToken cancellationToken = default);

    /// <summary>Confirms, rejects, cancels or completes a booking.</summary>
    Task<Result<BookingAggregate>> Handle(UpdateBookingStatusCommand command,
        CancellationToken cancellationToken = default);
}