using Veygo.Platform.Fleet.Interfaces.ACL;

namespace Veygo.Platform.Engagement.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service that queries the Fleet context through its anti-corruption layer.
/// </summary>
public class ExternalFleetService(IFleetContextFacade fleetContextFacade)
{
    /// <summary>Checks if a vehicle exists.</summary>
    public Task<bool> ExistsVehicleAsync(int vehicleId, CancellationToken cancellationToken = default) =>
        fleetContextFacade.ExistsVehicleByIdAsync(vehicleId, cancellationToken);
}
