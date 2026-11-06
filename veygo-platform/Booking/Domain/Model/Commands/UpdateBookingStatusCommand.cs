using Veygo.Platform.Booking.Domain.Model.ValueObjects;

namespace Veygo.Platform.Booking.Domain.Model.Commands;

/// <summary>
///     Command to move a booking to a new status (confirm, reject, cancel or complete).
/// </summary>
public record UpdateBookingStatusCommand(int BookingId, EBookingStatus Status);