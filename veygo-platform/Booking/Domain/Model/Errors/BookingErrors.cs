using Veygo.Platform.Shared.Domain.Model.Errors;

namespace Veygo.Platform.Booking.Domain.Model.Errors;

/// <summary>
///     Error catalog of the Booking bounded context.
/// </summary>
public static class BookingErrors
{
    public static readonly Error BookingNotFound =
        Error.NotFound("Booking.BookingNotFound", "The booking was not found.");

    public static readonly Error VehicleNotFound =
        Error.NotFound("Booking.VehicleNotFound", "The vehicle to book was not found.");

    public static readonly Error VehicleNotPublished =
        Error.Conflict("Booking.VehicleNotPublished", "The vehicle is not published for rental.");

    public static readonly Error VehicleNotAvailable =
        Error.Conflict("Booking.VehicleNotAvailable", "The vehicle is not available for the selected dates.");

    public static readonly Error OwnVehicle =
        Error.Validation("Booking.OwnVehicle", "Owners cannot book their own vehicles.");

    public static readonly Error StartDateInThePast =
        Error.Validation("Booking.StartDateInThePast", "The rental period must start today or later.");

    public static readonly Error InvalidStatusTransition =
        Error.Conflict("Booking.InvalidStatusTransition", "The booking cannot change to the requested status.");
}