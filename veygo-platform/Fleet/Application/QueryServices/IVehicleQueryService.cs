using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Queries;

namespace Veygo.Platform.Fleet.Application.QueryServices;

/// <summary>
///     Query service for vehicles.
/// </summary>
public interface IVehicleQueryService
{
    Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> Handle(GetPublishedVehiclesQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> Handle(GetVehiclesByOwnerIdQuery query, CancellationToken cancellationToken = default);
    Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default);
}
