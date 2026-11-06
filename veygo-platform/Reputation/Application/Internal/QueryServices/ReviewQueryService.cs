using Veygo.Platform.Reputation.Application.QueryServices;
using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Reputation.Domain.Model.Queries;
using Veygo.Platform.Reputation.Domain.Repositories;

namespace Veygo.Platform.Reputation.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for reviews.
/// </summary>
public class ReviewQueryService(IReviewRepository reviewRepository) : IReviewQueryService
{
    public async Task<IEnumerable<Review>> Handle(GetAllReviewsQuery query,
        CancellationToken cancellationToken = default) =>
        await reviewRepository.FindAllAsync(cancellationToken);

    public async Task<Review?> Handle(GetReviewByIdQuery query, CancellationToken cancellationToken = default) =>
        await reviewRepository.FindByIdAsNoTrackingAsync(query.ReviewId, cancellationToken);

    public async Task<IEnumerable<Review>> Handle(GetReviewsByOwnerIdQuery query,
        CancellationToken cancellationToken = default) =>
        await reviewRepository.FindByOwnerIdAsync(query.OwnerId, cancellationToken);

    public async Task<IEnumerable<Review>> Handle(GetReviewsByRenterIdQuery query,
        CancellationToken cancellationToken = default) =>
        await reviewRepository.FindByRenterIdAsync(query.RenterId, cancellationToken);

    public async Task<IEnumerable<Review>> Handle(GetReviewsByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await reviewRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);
}
