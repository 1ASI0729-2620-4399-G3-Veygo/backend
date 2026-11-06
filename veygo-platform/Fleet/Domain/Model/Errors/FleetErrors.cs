using Veygo.Platform.Shared.Domain.Model.Errors;

namespace Veygo.Platform.Fleet.Domain.Model.Errors;

/// <summary>
///     Error catalog of the Fleet bounded context.
/// </summary>
public static class FleetErrors
{
    public static readonly Error VehicleNotFound =
        Error.NotFound("Fleet.VehicleNotFound", "The vehicle was not found.");

    public static readonly Error DuplicatePlate =
        Error.Conflict("Fleet.DuplicatePlate", "Another vehicle is already registered with this plate.");

    public static readonly Error BlockedDateNotFound =
        Error.NotFound("Fleet.BlockedDateNotFound", "The blocked date was not found.");

    public static readonly Error DateAlreadyBlocked =
        Error.Conflict("Fleet.DateAlreadyBlocked", "The date is already blocked for this vehicle.");

    public static readonly Error BlockedDateInThePast =
        Error.Validation("Fleet.BlockedDateInThePast", "Past dates cannot be blocked.");
}
