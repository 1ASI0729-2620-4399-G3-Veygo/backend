using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Interfaces.Rest.Resources;

namespace Veygo.Platform.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="ConversationResource" /> from a <see cref="Conversation" />.
/// </summary>
public static class ConversationResourceFromEntityAssembler
{
    public static ConversationResource ToResourceFromEntity(Conversation entity) =>
        new(entity.Id, entity.RenterId, entity.OwnerId, entity.VehicleId, entity.CreatedAt,
            entity.LastMessageAt ?? entity.CreatedAt, entity.LastMessagePreview);
}
