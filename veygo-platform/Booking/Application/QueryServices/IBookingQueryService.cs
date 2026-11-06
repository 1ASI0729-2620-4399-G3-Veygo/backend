using Veygo.Platform.Booking.Application.QueryServices;
using Veygo.Platform.Booking.Domain.Model.Queries;
using Veygo.Platform.Booking.Domain.Repositories;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for bookings. All reads are non-tracking.
/// </summary>
public class BookingQueryService(IBookingRepository bookingRepository) : IBookingQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<BookingAggregate>> Handle(GetAllBookingsQuery query,
        CancellationToken cancellationToken = default) =>
        await bookingRepository.ListAsNoTrackingAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<BookingAggregate?> Handle(GetBookingByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await bookingRepository.FindByIdAsNoTrackingAsync(query.BookingId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<BookingAggregate>> Handle(GetBookingsByRenterIdQuery query,
        CancellationToken cancellationToken = default) =>
        await bookingRepository.FindByRenterIdAsync(query.RenterId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<BookingAggregate>> Handle(GetBookingsByOwnerIdQuery query,
        CancellationToken cancellationToken = default) =>
        await bookingRepository.FindByOwnerIdAsync(query.OwnerId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<BookingAggregate>> Handle(GetBookingsByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await bookingRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);
}