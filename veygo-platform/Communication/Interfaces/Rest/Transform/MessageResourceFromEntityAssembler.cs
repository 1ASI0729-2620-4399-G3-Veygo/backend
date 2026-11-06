using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Interfaces.Rest.Resources;

namespace Veygo.Platform.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="MessageResource" /> from a <see cref="Message" />.
/// </summary>
public static class MessageResourceFromEntityAssembler
{
    public static MessageResource ToResourceFromEntity(Message entity) =>
        new(entity.Id, entity.ConversationId, entity.SenderId, entity.Content, entity.CreatedAt, entity.Read);
}
