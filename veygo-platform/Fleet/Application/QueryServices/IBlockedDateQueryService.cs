using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Queries;

namespace Veygo.Platform.Fleet.Application.QueryServices;

/// <summary>
///     Query service for blocked days.
/// </summary>
public interface IBlockedDateQueryService
{
    Task<IEnumerable<BlockedDate>> Handle(GetBlockedDatesByVehicleIdQuery query,
        CancellationToken cancellationToken = default);

    Task<BlockedDate?> Handle(GetBlockedDateByIdQuery query, CancellationToken cancellationToken = default);
}
