namespace Veygo.Platform.Booking.Domain.Model.Queries;

/// <summary>
///     Query to get a booking by its id.
/// </summary>
public record GetBookingByIdQuery(int BookingId);