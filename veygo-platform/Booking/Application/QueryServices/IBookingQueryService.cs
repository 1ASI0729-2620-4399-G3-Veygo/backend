using Veygo.Platform.Booking.Domain.Model.Queries;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Application.QueryServices;

/// <summary>
///     Query service for bookings.
/// </summary>
public interface IBookingQueryService
{
    Task<IEnumerable<BookingAggregate>> Handle(GetAllBookingsQuery query, CancellationToken cancellationToken = default);
    Task<BookingAggregate?> Handle(GetBookingByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingAggregate>> Handle(GetBookingsByRenterIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingAggregate>> Handle(GetBookingsByOwnerIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingAggregate>> Handle(GetBookingsByVehicleIdQuery query, CancellationToken cancellationToken = default);
}