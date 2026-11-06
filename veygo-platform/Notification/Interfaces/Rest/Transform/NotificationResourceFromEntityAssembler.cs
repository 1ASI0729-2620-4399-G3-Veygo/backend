using System.Text.Json;
using Veygo.Platform.Notification.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="NotificationResource" /> from a notification.
/// </summary>
public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(NotificationAggregate entity) =>
        new(entity.Id,
            entity.UserId,
            EnumValueAssembler.ToValue(entity.Type),
            ParseJson(entity.Params) ?? ParseJson("{}")!.Value,
            ParseJson(entity.Link),
            entity.Read,
            entity.CreatedAt);

    private static JsonElement? ParseJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }
}
