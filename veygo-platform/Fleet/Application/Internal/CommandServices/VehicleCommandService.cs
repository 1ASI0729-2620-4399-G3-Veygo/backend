using Veygo.Platform.Fleet.Application.CommandServices;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Fleet.Domain.Model.Errors;
using Veygo.Platform.Fleet.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Fleet.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for vehicles.
/// </summary>
public class VehicleCommandService(
    IVehicleRepository vehicleRepository,
    IBlockedDateRepository blockedDateRepository,
    IUnitOfWork unitOfWork)
    : IVehicleCommandService
{
    /// <inheritdoc />
    public async Task<Result<Vehicle>> Handle(CreateVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        var vehicle = new Vehicle(command);
        if (await IsPlateTakenAsync(vehicle.Plate, null, cancellationToken))
            return Result<Vehicle>.Failure(FleetErrors.DuplicatePlate);

        await vehicleRepository.AddAsync(vehicle, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Vehicle>.Success(vehicle);
    }

    /// <inheritdoc />
    public async Task<Result<Vehicle>> Handle(UpdateVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle is null) return Result<Vehicle>.Failure(FleetErrors.VehicleNotFound);

        vehicle.Update(command);
        if (await IsPlateTakenAsync(vehicle.Plate, vehicle.Id, cancellationToken))
            return Result<Vehicle>.Failure(FleetErrors.DuplicatePlate);

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Vehicle>.Success(vehicle);
    }

    /// <inheritdoc />
    public async Task<Result<Vehicle>> Handle(UpdateVehiclePublicationCommand command,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle is null) return Result<Vehicle>.Failure(FleetErrors.VehicleNotFound);

        if (command.Published) vehicle.Publish();
        else vehicle.Unpublish();

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Vehicle>.Success(vehicle);
    }

    /// <inheritdoc />
    public async Task<Result<Vehicle>> Handle(UpdateVehicleReputationCommand command,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle is null) return Result<Vehicle>.Failure(FleetErrors.VehicleNotFound);

        vehicle.UpdateReputation(command.Rating, command.ReviewsCount);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Vehicle>.Success(vehicle);
    }

    /// <inheritdoc />
    public async Task<Result<Vehicle>> Handle(DeleteVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId, cancellationToken);
        if (vehicle is null) return Result<Vehicle>.Failure(FleetErrors.VehicleNotFound);

        foreach (var blockedDate in await blockedDateRepository.FindByVehicleIdAsync(vehicle.Id, cancellationToken))
            blockedDateRepository.Remove(blockedDate);
        vehicleRepository.Remove(vehicle);

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Vehicle>.Success(vehicle);
    }

    private async Task<bool> IsPlateTakenAsync(string plate, int? excludedVehicleId,
        CancellationToken cancellationToken) =>
        plate.Length > 0 && await vehicleRepository.ExistsByPlateAsync(plate, excludedVehicleId, cancellationToken);
}
