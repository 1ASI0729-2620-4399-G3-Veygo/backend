using Veygo.Platform.Engagement.Domain.Model.Aggregates;
using Veygo.Platform.Engagement.Interfaces.Rest.Resources;

namespace Veygo.Platform.Engagement.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="FavoriteResource" /> from a <see cref="Favorite" />.
/// </summary>
public static class FavoriteResourceFromEntityAssembler
{
    public static FavoriteResource ToResourceFromEntity(Favorite entity) =>
        new(entity.Id, entity.RenterId, entity.VehicleId, entity.CreatedAt);
}
