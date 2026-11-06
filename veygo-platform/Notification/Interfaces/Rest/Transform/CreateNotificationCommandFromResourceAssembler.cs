using System.Text.Json;
using Veygo.Platform.Notification.Domain.Model.Commands;
using Veygo.Platform.Notification.Domain.Model.ValueObjects;
using Veygo.Platform.Notification.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;

namespace Veygo.Platform.Notification.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="CreateNotificationCommand" /> from a <see cref="CreateNotificationResource" />.
/// </summary>
public static class CreateNotificationCommandFromResourceAssembler
{
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource resource) =>
        new(resource.UserId,
            EnumValueAssembler.ToEnum<ENotificationType>(resource.Type, "type"),
            ToJson(resource.Params),
            ToJson(resource.Link));

    private static string? ToJson(JsonElement? element) =>
        element is null || element.Value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined
            ? null
            : element.Value.GetRawText();
}
