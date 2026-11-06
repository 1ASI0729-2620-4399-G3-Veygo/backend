using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Reputation.Domain.Model.Queries;

namespace Veygo.Platform.Reputation.Application.QueryServices;

/// <summary>
///     Query service for reviews.
/// </summary>
public interface IReviewQueryService
{
    Task<IEnumerable<Review>> Handle(GetAllReviewsQuery query, CancellationToken cancellationToken = default);
    Task<Review?> Handle(GetReviewByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> Handle(GetReviewsByOwnerIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> Handle(GetReviewsByRenterIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> Handle(GetReviewsByVehicleIdQuery query, CancellationToken cancellationToken = default);
}
