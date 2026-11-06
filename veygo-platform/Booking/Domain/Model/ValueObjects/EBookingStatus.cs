namespace Veygo.Platform.Booking.Domain.Model.ValueObjects;

/// <summary>
///     Lifecycle of a booking: a renter requests it (pending), the owner confirms or rejects it,
///     it can be cancelled before it starts and it is completed after the rental.
/// </summary>
public enum EBookingStatus
{
    Pending,
    Confirmed,
    Rejected,
    Cancelled,
    Completed
}