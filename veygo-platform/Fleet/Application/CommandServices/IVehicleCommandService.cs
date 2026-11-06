using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;

namespace Veygo.Platform.Fleet.Application.CommandServices;

/// <summary>
///     Command service for vehicles.
/// </summary>
public interface IVehicleCommandService
{
    /// <summary>Registers a new vehicle. Fails with a conflict when the plate is already registered.</summary>
    Task<Result<Vehicle>> Handle(CreateVehicleCommand command, CancellationToken cancellationToken = default);

    /// <summary>Replaces the editable data of a vehicle.</summary>
    Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken = default);

    /// <summary>Publishes or unpublishes a vehicle.</summary>
    Task<Result<Vehicle>> Handle(UpdateVehiclePublicationCommand command, CancellationToken cancellationToken = default);

    /// <summary>Updates the rating summary of a vehicle.</summary>
    Task<Result<Vehicle>> Handle(UpdateVehicleReputationCommand command, CancellationToken cancellationToken = default);

    /// <summary>Deletes a vehicle and its blocked days.</summary>
    Task<Result<Vehicle>> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken = default);
}
