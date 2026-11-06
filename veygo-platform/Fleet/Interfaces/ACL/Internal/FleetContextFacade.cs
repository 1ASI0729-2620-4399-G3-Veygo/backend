using Veygo.Platform.Fleet.Application.QueryServices;
using Veygo.Platform.Fleet.Domain.Model.Queries;
using Veygo.Platform.Fleet.Domain.Repositories;

namespace Veygo.Platform.Fleet.Interfaces.ACL.Internal;

/// <summary>
///     Implementation of <see cref="IFleetContextFacade" />.
/// </summary>
public class FleetContextFacade(
    IVehicleQueryService vehicleQueryService,
    IBlockedDateRepository blockedDateRepository)
    : IFleetContextFacade
{
    /// <inheritdoc />
    public async Task<(int OwnerId, decimal PricePerDay, bool Published)?> FetchVehicleRentalTermsAsync(
        int vehicleId, CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleQueryService.Handle(new GetVehicleByIdQuery(vehicleId), cancellationToken);
        if (vehicle is null) return null;
        return (vehicle.OwnerId, vehicle.PricePerDay, vehicle.Published);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsVehicleByIdAsync(int vehicleId, CancellationToken cancellationToken = default) =>
        await vehicleQueryService.Handle(new GetVehicleByIdQuery(vehicleId), cancellationToken) is not null;

    /// <inheritdoc />
    public async Task<bool> HasBlockedDatesBetweenAsync(int vehicleId, DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken = default) =>
        await blockedDateRepository.ExistsInRangeAsync(vehicleId, startDate, endDate, cancellationToken);
}
