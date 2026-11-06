using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Booking.Domain.Model.ValueObjects;
using Veygo.Platform.Booking.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IBookingRepository" />.
/// </summary>
public class BookingRepository(AppDbContext context) : BaseRepository<BookingAggregate>(context), IBookingRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<BookingAggregate>> FindByRenterIdAsync(int renterId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<BookingAggregate>().AsNoTracking()
            .Where(booking => booking.RenterId == renterId)
            .OrderByDescending(booking => booking.StartDate)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<BookingAggregate>> FindByOwnerIdAsync(int ownerId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<BookingAggregate>().AsNoTracking()
            .Where(booking => booking.OwnerId == ownerId)
            .OrderByDescending(booking => booking.StartDate)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<BookingAggregate>> FindByVehicleIdAsync(int vehicleId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<BookingAggregate>().AsNoTracking()
            .Where(booking => booking.VehicleId == vehicleId)
            .OrderBy(booking => booking.StartDate)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> ExistsActiveOverlappingAsync(int vehicleId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken = default) =>
        await Context.Set<BookingAggregate>().AnyAsync(booking =>
                booking.VehicleId == vehicleId &&
                (booking.Status == EBookingStatus.Pending || booking.Status == EBookingStatus.Confirmed) &&
                booking.StartDate < endDate && startDate < booking.EndDate,
            cancellationToken);
}