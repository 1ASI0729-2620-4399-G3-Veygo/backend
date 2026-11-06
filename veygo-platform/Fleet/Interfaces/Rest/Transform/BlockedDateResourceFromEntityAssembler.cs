using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;

namespace Veygo.Platform.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="BlockedDateResource" /> from a <see cref="BlockedDate" />.
/// </summary>
public static class BlockedDateResourceFromEntityAssembler
{
    public static BlockedDateResource ToResourceFromEntity(BlockedDate entity) =>
        new(entity.Id, entity.VehicleId, DateValueAssembler.ToValue(entity.Date));
}
