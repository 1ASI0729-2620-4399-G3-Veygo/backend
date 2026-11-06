using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Communication.Interfaces.Rest.Resources;

namespace Veygo.Platform.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="CreateMessageCommand" /> from a <see cref="CreateMessageResource" />.
/// </summary>
public static class CreateMessageCommandFromResourceAssembler
{
    public static CreateMessageCommand ToCommandFromResource(CreateMessageResource resource) =>
        new(resource.ConversationId, resource.SenderId, resource.Content);
}
