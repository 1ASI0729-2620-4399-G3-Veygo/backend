namespace Veygo.Platform.Booking.Domain.Model.Queries;

/// <summary>
///     Query to get the bookings of a vehicle.
/// </summary>
public record GetBookingsByVehicleIdQuery(int VehicleId);