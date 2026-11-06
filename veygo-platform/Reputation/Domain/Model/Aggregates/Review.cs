using Veygo.Platform.Reputation.Domain.Model.Commands;
using Veygo.Platform.Reputation.Domain.Model.ValueObjects;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Reputation.Domain.Model.Aggregates;

/// <summary>
///     Review aggregate root: the rating (1 to 5) a renter gives to a finished rental.
/// </summary>
public class Review : AggregateRoot, IAuditableEntity
{
    /// <summary>Maximum length of the comment.</summary>
    public const int MaxCommentLength = 500;

    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected Review()
    {
        Comment = string.Empty;
    }

    /// <summary>
    ///     Creates a review of a finished rental.
    /// </summary>
    /// <param name="command">The review data.</param>
    /// <param name="booking">The reviewed booking.</param>
    /// <exception cref="ArgumentException">When a domain invariant is violated.</exception>
    public Review(CreateReviewCommand command, BookingSummary booking)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(booking);
        if (command.BookingId != booking.BookingId)
            throw new ArgumentException("The booking does not match the review.", nameof(booking));
        if (command.Rating is < 1 or > 5)
            throw new ArgumentException("The rating must be an integer from 1 to 5.", nameof(command));
        var comment = command.Comment?.Trim() ?? string.Empty;
        if (comment.Length > MaxCommentLength)
            throw new ArgumentException($"The comment must have at most {MaxCommentLength} characters.",
                nameof(command));

        BookingId = booking.BookingId;
        VehicleId = booking.VehicleId;
        OwnerId = booking.OwnerId;
        RenterId = booking.RenterId;
        Rating = command.Rating;
        Comment = comment;
    }

    public int Id { get; private set; }
    public int BookingId { get; private set; }
    public int VehicleId { get; private set; }
    public int OwnerId { get; private set; }
    public int RenterId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Gets a value indicating whether the review is positive (4 or 5 stars).</summary>
    public bool IsPositive => Rating >= 4;
}
