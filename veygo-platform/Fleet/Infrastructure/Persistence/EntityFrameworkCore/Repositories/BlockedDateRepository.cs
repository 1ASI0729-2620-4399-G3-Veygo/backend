using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IBlockedDateRepository" />.
/// </summary>
public class BlockedDateRepository(AppDbContext context)
    : BaseRepository<BlockedDate>(context), IBlockedDateRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<BlockedDate>> FindByVehicleIdAsync(int vehicleId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<BlockedDate>().AsNoTracking()
            .Where(blockedDate => blockedDate.VehicleId == vehicleId)
            .OrderBy(blockedDate => blockedDate.Date)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> ExistsByVehicleIdAndDateAsync(int vehicleId, DateOnly date,
        CancellationToken cancellationToken = default) =>
        await Context.Set<BlockedDate>()
            .AnyAsync(blockedDate => blockedDate.VehicleId == vehicleId && blockedDate.Date == date, cancellationToken);

    /// <inheritdoc />
    public async Task<bool> ExistsInRangeAsync(int vehicleId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken = default) =>
        await Context.Set<BlockedDate>().AnyAsync(
            blockedDate => blockedDate.VehicleId == vehicleId && blockedDate.Date >= startDate &&
                           blockedDate.Date <= endDate,
            cancellationToken);
}
