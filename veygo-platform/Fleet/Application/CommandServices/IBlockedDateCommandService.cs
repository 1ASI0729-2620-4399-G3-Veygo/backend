using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;

namespace Veygo.Platform.Fleet.Application.CommandServices;

/// <summary>
///     Command service for the availability calendar (blocked days) of vehicles.
/// </summary>
public interface IBlockedDateCommandService
{
    /// <summary>Blocks a day for a vehicle.</summary>
    Task<Result<BlockedDate>> Handle(CreateBlockedDateCommand command, CancellationToken cancellationToken = default);

    /// <summary>Releases a blocked day.</summary>
    Task<Result<BlockedDate>> Handle(DeleteBlockedDateCommand command, CancellationToken cancellationToken = default);
}
