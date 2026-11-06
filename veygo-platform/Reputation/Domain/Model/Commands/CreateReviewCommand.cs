namespace Veygo.Platform.Reputation.Domain.Model.Commands;

/// <summary>
///     Command for a renter to rate a finished rental. The vehicle and the owner are taken from the booking.
/// </summary>
public record CreateReviewCommand(int BookingId, int RenterId, int Rating, string Comment);
