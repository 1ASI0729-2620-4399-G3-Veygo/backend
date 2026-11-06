using Veygo.Platform.Shared.Domain.Repositories;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Domain.Repositories;

/// <summary>
///     Repository of bookings.
/// </summary>
/// <remarks>
///     The alias <c>BookingAggregate</c> avoids the clash between the <c>Booking</c> class and the
///     <c>Veygo.Platform.Booking</c> namespace.
/// </remarks>
public interface IBookingRepository : IBaseRepository<BookingAggregate>
{
    /// <summary>Lists the bookings of a renter, most recent rental first (read only).</summary>
    Task<IEnumerable<BookingAggregate>> FindByRenterIdAsync(int renterId, CancellationToken cancellationToken = default);

    /// <summary>Lists the bookings received by an owner, most recent rental first (read only).</summary>
    Task<IEnumerable<BookingAggregate>> FindByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default);

    /// <summary>Lists the bookings of a vehicle (read only).</summary>
    Task<IEnumerable<BookingAggregate>> FindByVehicleIdAsync(int vehicleId,
        CancellationToken cancellationToken = default);

    /// <summary>Checks if a pending or confirmed booking of the vehicle overlaps the period.</summary>
    Task<bool> ExistsActiveOverlappingAsync(int vehicleId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken = default);
}