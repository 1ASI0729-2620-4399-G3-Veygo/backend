namespace Veygo.Platform.Booking.Domain.Model.Queries;

/// <summary>
///     Query to get the bookings of the vehicles of an owner (most recent rental first).
/// </summary>
public record GetBookingsByOwnerIdQuery(int OwnerId);