using Veygo.Platform.Reputation.Application.CommandServices;
using Veygo.Platform.Reputation.Application.Internal.OutboundServices;
using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Reputation.Domain.Model.Commands;
using Veygo.Platform.Reputation.Domain.Model.Errors;
using Veygo.Platform.Reputation.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Reputation.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for reviews.
/// </summary>
public class ReviewCommandService(
    IReviewRepository reviewRepository,
    ExternalBookingService externalBookingService,
    IUnitOfWork unitOfWork)
    : IReviewCommandService
{
    /// <inheritdoc />
    public async Task<Result<Review>> Handle(CreateReviewCommand command, CancellationToken cancellationToken = default)
    {
        var booking = await externalBookingService.FetchBookingSummaryAsync(command.BookingId, cancellationToken);
        if (booking is null) return Result<Review>.Failure(ReputationErrors.BookingNotFound);
        if (booking.RenterId != command.RenterId) return Result<Review>.Failure(ReputationErrors.NotBookingRenter);
        if (!booking.IsFinished) return Result<Review>.Failure(ReputationErrors.RentalNotFinished);
        if (await reviewRepository.ExistsByBookingIdAsync(command.BookingId, cancellationToken))
            return Result<Review>.Failure(ReputationErrors.DuplicateReview);

        var review = new Review(command, booking);
        await reviewRepository.AddAsync(review, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Review>.Success(review);
    }
}
