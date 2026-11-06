using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Reputation.Interfaces.Rest.Resources;

/// <summary>
///     Data required to review a finished rental. The vehicle and the owner are taken from the booking.
/// </summary>
public record CreateReviewResource(
    [Range(1, int.MaxValue)] int BookingId,
    [Range(1, int.MaxValue)] int RenterId,
    [Range(1, 5)] int Rating,
    [MaxLength(500)] string? Comment);
