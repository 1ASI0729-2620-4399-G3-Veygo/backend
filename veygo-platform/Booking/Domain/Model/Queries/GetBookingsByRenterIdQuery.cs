namespace Veygo.Platform.Booking.Domain.Model.Queries;

/// <summary>
///     Query to get the bookings of a renter (most recent rental first).
/// </summary>
public record GetBookingsByRenterIdQuery(int RenterId);