using Veygo.Platform.Booking.Domain.Model.ValueObjects;

namespace Veygo.Platform.Booking.Domain.Model.Commands;

/// <summary>
///     Command for a renter to request a vehicle for a rental period. The owner and the prices are taken
///     from the vehicle, never from the client.
/// </summary>
public record CreateBookingCommand(
    int VehicleId,
    int RenterId,
    DateOnly StartDate,
    DateOnly EndDate,
    EPaymentMethod PaymentMethod);