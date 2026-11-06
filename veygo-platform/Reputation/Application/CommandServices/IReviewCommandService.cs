using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Reputation.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;

namespace Veygo.Platform.Reputation.Application.CommandServices;

/// <summary>
///     Command service for reviews.
/// </summary>
public interface IReviewCommandService
{
    /// <summary>
    ///     Publishes a review. The booking must exist, belong to the renter, be finished and not reviewed yet.
    /// </summary>
    Task<Result<Review>> Handle(CreateReviewCommand command, CancellationToken cancellationToken = default);
}
