using Veygo.Platform.Engagement.Domain.Model.Aggregates;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Engagement.Domain.Repositories;

/// <summary>
///     Repository of favorites.
/// </summary>
public interface IFavoriteRepository : IBaseRepository<Favorite>
{
    /// <summary>Lists the favorites of a renter, most recent first (read only).</summary>
    Task<IEnumerable<Favorite>> FindByRenterIdAsync(int renterId, CancellationToken cancellationToken = default);

    /// <summary>Checks if the renter already saved the vehicle.</summary>
    Task<bool> ExistsByRenterIdAndVehicleIdAsync(int renterId, int vehicleId,
        CancellationToken cancellationToken = default);
}
