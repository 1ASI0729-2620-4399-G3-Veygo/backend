using Veygo.Platform.Shared.Domain.Model.Errors;

namespace Veygo.Platform.Reputation.Domain.Model.Errors;

/// <summary>
///     Error catalog of the Reputation bounded context.
/// </summary>
public static class ReputationErrors
{
    public static readonly Error BookingNotFound =
        Error.NotFound("Reputation.BookingNotFound", "The booking to review was not found.");

    public static readonly Error NotBookingRenter =
        Error.Forbidden("Reputation.NotBookingRenter", "Only the renter of the booking can review it.");

    public static readonly Error RentalNotFinished =
        Error.Failure("Reputation.RentalNotFinished", "Only finished rentals can be reviewed.");

    public static readonly Error DuplicateReview =
        Error.Conflict("Reputation.DuplicateReview", "The booking was already reviewed.");
}
