using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Engagement.Domain.Model.Aggregates;
using Veygo.Platform.Engagement.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IFavoriteRepository" />.
/// </summary>
public class FavoriteRepository(AppDbContext context) : BaseRepository<Favorite>(context), IFavoriteRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Favorite>> FindByRenterIdAsync(int renterId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Favorite>().AsNoTracking()
            .Where(favorite => favorite.RenterId == renterId)
            .OrderByDescending(favorite => favorite.CreatedAt)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> ExistsByRenterIdAndVehicleIdAsync(int renterId, int vehicleId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Favorite>().AnyAsync(
            favorite => favorite.RenterId == renterId && favorite.VehicleId == vehicleId, cancellationToken);
}
