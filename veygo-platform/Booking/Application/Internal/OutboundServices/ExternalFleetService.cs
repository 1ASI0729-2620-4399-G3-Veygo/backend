using Veygo.Platform.Booking.Domain.Model.ValueObjects;
using Veygo.Platform.Fleet.Interfaces.ACL;

namespace Veygo.Platform.Booking.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service that queries the Fleet context through its anti-corruption layer and translates
///     the answers into Booking domain objects.
/// </summary>
public class ExternalFleetService(IFleetContextFacade fleetContextFacade)
{
    /// <summary>
    ///     Gets the rental terms of a vehicle, or null when it does not exist.
    /// </summary>
    public async Task<VehicleRentalTerms?> FetchVehicleRentalTermsAsync(int vehicleId,
        CancellationToken cancellationToken = default)
    {
        var terms = await fleetContextFacade.FetchVehicleRentalTermsAsync(vehicleId, cancellationToken);
        return terms is null
            ? null
            : new VehicleRentalTerms(vehicleId, terms.Value.OwnerId, terms.Value.PricePerDay, terms.Value.Published);
    }

    /// <summary>
    ///     Checks if the owner blocked any of the given nights.
    /// </summary>
    public Task<bool> HasBlockedNightsAsync(int vehicleId, DateOnly firstNight, DateOnly lastNight,
        CancellationToken cancellationToken = default) =>
        fleetContextFacade.HasBlockedDatesBetweenAsync(vehicleId, firstNight, lastNight, cancellationToken);
}