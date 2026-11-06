namespace Veygo.Platform.Fleet.Interfaces.ACL;

/// <summary>
///     Anti-corruption layer facade that the other bounded contexts use to query the Fleet context.
/// </summary>
/// <remarks>Only primitive values cross the boundary, never Fleet domain objects.</remarks>
public interface IFleetContextFacade
{
    /// <summary>
    ///     Gets the rental terms of a vehicle, or null when the vehicle does not exist.
    /// </summary>
    /// <param name="vehicleId">The vehicle id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The owner id, the price per day and whether the vehicle is published.</returns>
    Task<(int OwnerId, decimal PricePerDay, bool Published)?> FetchVehicleRentalTermsAsync(int vehicleId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks if a vehicle exists.
    /// </summary>
    Task<bool> ExistsVehicleByIdAsync(int vehicleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks if any day of the period (inclusive) is blocked by the owner.
    /// </summary>
    Task<bool> HasBlockedDatesBetweenAsync(int vehicleId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken = default);
}
