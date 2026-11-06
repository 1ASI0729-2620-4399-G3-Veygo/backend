using Veygo.Platform.Engagement.Domain.Model.Commands;
using Veygo.Platform.Engagement.Interfaces.Rest.Resources;

namespace Veygo.Platform.Engagement.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="CreateFavoriteCommand" /> from a <see cref="CreateFavoriteResource" />.
/// </summary>
public static class CreateFavoriteCommandFromResourceAssembler
{
    public static CreateFavoriteCommand ToCommandFromResource(CreateFavoriteResource resource) =>
        new(resource.RenterId, resource.VehicleId);
}
