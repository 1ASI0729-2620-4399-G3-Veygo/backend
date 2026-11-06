using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Reputation.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Reputation.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IReviewRepository" />.
/// </summary>
public class ReviewRepository(AppDbContext context) : BaseRepository<Review>(context), IReviewRepository
{
    public async Task<IEnumerable<Review>> FindAllAsync(CancellationToken cancellationToken = default) =>
        await Latest(Context.Set<Review>().AsNoTracking()).ToListAsync(cancellationToken);

    public async Task<IEnumerable<Review>> FindByOwnerIdAsync(int ownerId,
        CancellationToken cancellationToken = default) =>
        await Latest(Context.Set<Review>().AsNoTracking().Where(review => review.OwnerId == ownerId))
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Review>> FindByRenterIdAsync(int renterId,
        CancellationToken cancellationToken = default) =>
        await Latest(Context.Set<Review>().AsNoTracking().Where(review => review.RenterId == renterId))
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Review>> FindByVehicleIdAsync(int vehicleId,
        CancellationToken cancellationToken = default) =>
        await Latest(Context.Set<Review>().AsNoTracking().Where(review => review.VehicleId == vehicleId))
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default) =>
        await Context.Set<Review>().AnyAsync(review => review.BookingId == bookingId, cancellationToken);

    private static IQueryable<Review> Latest(IQueryable<Review> reviews) =>
        reviews.OrderByDescending(review => review.CreatedAt).ThenByDescending(review => review.Id);
}
