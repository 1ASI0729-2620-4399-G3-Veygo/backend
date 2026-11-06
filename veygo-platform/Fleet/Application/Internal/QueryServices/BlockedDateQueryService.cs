using Veygo.Platform.Fleet.Application.QueryServices;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Queries;
using Veygo.Platform.Fleet.Domain.Repositories;

namespace Veygo.Platform.Fleet.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for blocked days.
/// </summary>
public class BlockedDateQueryService(IBlockedDateRepository blockedDateRepository) : IBlockedDateQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<BlockedDate>> Handle(GetBlockedDatesByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await blockedDateRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);

    /// <inheritdoc />
    public async Task<BlockedDate?> Handle(GetBlockedDateByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await blockedDateRepository.FindByIdAsNoTrackingAsync(query.BlockedDateId, cancellationToken);
}
