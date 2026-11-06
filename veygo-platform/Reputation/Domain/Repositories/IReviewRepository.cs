using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Reputation.Domain.Repositories;

/// <summary>
///     Repository of reviews. All list methods return the most recent reviews first (read only).
/// </summary>
public interface IReviewRepository : IBaseRepository<Review>
{
    Task<IEnumerable<Review>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> FindByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> FindByRenterIdAsync(int renterId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default);

    /// <summary>Checks if a booking was already reviewed.</summary>
    Task<bool> ExistsByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default);
}
