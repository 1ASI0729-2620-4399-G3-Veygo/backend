using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Fleet.Domain.Repositories;

/// <summary>
///     Repository of vehicles.
/// </summary>
public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    /// <summary>Lists the vehicles of an owner (read only).</summary>
    Task<IEnumerable<Vehicle>> FindByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default);

    /// <summary>Lists the vehicles published in the catalog (read only).</summary>
    Task<IEnumerable<Vehicle>> FindPublishedAsync(CancellationToken cancellationToken = default);

    /// <summary>Checks if another vehicle already uses the plate.</summary>
    /// <param name="plate">The plate to check.</param>
    /// <param name="excludedVehicleId">A vehicle to ignore (the one being updated), if any.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> ExistsByPlateAsync(string plate, int? excludedVehicleId = null,
        CancellationToken cancellationToken = default);
}
