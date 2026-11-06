using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Booking.Interfaces.Rest.Resources;

/// <summary>
///     Booking request. The owner and the prices are taken from the vehicle; if the client sends them they are ignored.
/// </summary>
public record CreateBookingResource(
    [Range(1, int.MaxValue)] int VehicleId,
    [Range(1, int.MaxValue)] int RenterId,
    [Required] string StartDate,
    [Required] string EndDate,
    string? PaymentMethod);