using Veygo.Platform.Fleet.Application.CommandServices;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Fleet.Domain.Model.Errors;
using Veygo.Platform.Fleet.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Model.ValueObjects;
using Veygo.Platform.Shared.Domain.Repositories;

namespace Veygo.Platform.Fleet.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for blocked days.
/// </summary>
public class BlockedDateCommandService(
    IBlockedDateRepository blockedDateRepository,
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork)
    : IBlockedDateCommandService
{
    /// <inheritdoc />
    public async Task<Result<BlockedDate>> Handle(CreateBlockedDateCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await vehicleRepository.FindByIdAsNoTrackingAsync(command.VehicleId, cancellationToken) is null)
            return Result<BlockedDate>.Failure(FleetErrors.VehicleNotFound);
        if (command.Date < BusinessCalendar.Today)
            return Result<BlockedDate>.Failure(FleetErrors.BlockedDateInThePast);
        if (await blockedDateRepository.ExistsByVehicleIdAndDateAsync(command.VehicleId, command.Date,
                cancellationToken))
            return Result<BlockedDate>.Failure(FleetErrors.DateAlreadyBlocked);

        var blockedDate = new BlockedDate(command);
        await blockedDateRepository.AddAsync(blockedDate, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<BlockedDate>.Success(blockedDate);
    }

    /// <inheritdoc />
    public async Task<Result<BlockedDate>> Handle(DeleteBlockedDateCommand command,
        CancellationToken cancellationToken = default)
    {
        var blockedDate = await blockedDateRepository.FindByIdAsync(command.BlockedDateId, cancellationToken);
        if (blockedDate is null) return Result<BlockedDate>.Failure(FleetErrors.BlockedDateNotFound);

        blockedDateRepository.Remove(blockedDate);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<BlockedDate>.Success(blockedDate);
    }
}
