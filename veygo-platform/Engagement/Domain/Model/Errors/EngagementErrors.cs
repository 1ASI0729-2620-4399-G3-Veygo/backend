using Veygo.Platform.Shared.Domain.Model.Errors;

namespace Veygo.Platform.Engagement.Domain.Model.Errors;

/// <summary>
///     Error catalog of the Engagement bounded context.
/// </summary>
public static class EngagementErrors
{
    public static readonly Error FavoriteNotFound =
        Error.NotFound("Engagement.FavoriteNotFound", "The favorite was not found.");

    public static readonly Error VehicleNotFound =
        Error.NotFound("Engagement.VehicleNotFound", "The vehicle was not found.");

    public static readonly Error DuplicateFavorite =
        Error.Conflict("Engagement.DuplicateFavorite", "The vehicle is already in the favorites of the renter.");
}
