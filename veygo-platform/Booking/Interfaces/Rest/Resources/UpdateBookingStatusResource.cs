using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Booking.Interfaces.Rest.Resources;

/// <summary>
///     New status of a booking: confirmed, rejected, cancelled or completed.
/// </summary>
public record UpdateBookingStatusResource([Required] string Status);