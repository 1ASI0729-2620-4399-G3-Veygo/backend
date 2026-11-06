using Veygo.Platform.Fleet.Application.QueryServices;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Queries;
using Veygo.Platform.Fleet.Domain.Repositories;

namespace Veygo.Platform.Fleet.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for vehicles. All reads are non-tracking.
/// </summary>
public class VehicleQueryService(IVehicleRepository vehicleRepository) : IVehicleQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query,
        CancellationToken cancellationToken = default) =>
        await vehicleRepository.ListAsNoTrackingAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> Handle(GetPublishedVehiclesQuery query,
        CancellationToken cancellationToken = default) =>
        await vehicleRepository.FindPublishedAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> Handle(GetVehiclesByOwnerIdQuery query,
        CancellationToken cancellationToken = default) =>
        await vehicleRepository.FindByOwnerIdAsync(query.OwnerId, cancellationToken);

    /// <inheritdoc />
    public async Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default) =>
        await vehicleRepository.FindByIdAsNoTrackingAsync(query.VehicleId, cancellationToken);
}
