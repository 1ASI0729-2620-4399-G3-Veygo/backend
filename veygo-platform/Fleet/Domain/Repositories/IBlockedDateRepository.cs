using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Fleet.Domain.Repositories;

/// <summary>
///     Repository of blocked days.
/// </summary>
public interface IBlockedDateRepository : IBaseRepository<BlockedDate>
{
    /// <summary>Lists the blocked days of a vehicle ordered by date (read only).</summary>
    Task<IEnumerable<BlockedDate>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default);

    /// <summary>Checks if a day is already blocked for a vehicle.</summary>
    Task<bool> ExistsByVehicleIdAndDateAsync(int vehicleId, DateOnly date,
        CancellationToken cancellationToken = default);

    /// <summary>Checks if any day between the two dates (inclusive) is blocked for a vehicle.</summary>
    Task<bool> ExistsInRangeAsync(int vehicleId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken = default);
}
