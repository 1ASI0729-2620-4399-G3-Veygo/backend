using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Communication.Interfaces.Rest.Resources;

namespace Veygo.Platform.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="CreateConversationCommand" /> from a <see cref="CreateConversationResource" />.
/// </summary>
public static class CreateConversationCommandFromResourceAssembler
{
    public static CreateConversationCommand ToCommandFromResource(CreateConversationResource resource) =>
        new(resource.RenterId, resource.OwnerId, resource.VehicleId);
}
