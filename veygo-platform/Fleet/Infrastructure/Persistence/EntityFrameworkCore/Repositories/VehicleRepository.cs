using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IVehicleRepository" />.
/// </summary>
public class VehicleRepository(AppDbContext context) : BaseRepository<Vehicle>(context), IVehicleRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> FindByOwnerIdAsync(int ownerId,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Vehicle>().AsNoTracking()
            .Where(vehicle => vehicle.OwnerId == ownerId)
            .OrderBy(vehicle => vehicle.Id)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> FindPublishedAsync(CancellationToken cancellationToken = default) =>
        await Context.Set<Vehicle>().AsNoTracking()
            .Where(vehicle => vehicle.Published)
            .OrderBy(vehicle => vehicle.Id)
            .ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> ExistsByPlateAsync(string plate, int? excludedVehicleId = null,
        CancellationToken cancellationToken = default) =>
        await Context.Set<Vehicle>().AnyAsync(
            vehicle => vehicle.Plate == plate && (excludedVehicleId == null || vehicle.Id != excludedVehicleId),
            cancellationToken);
}
