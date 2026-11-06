using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Veygo.Platform.Notification.Interfaces.Rest.Resources;

/// <summary>
///     Data required to notify a user. <c>params</c> and <c>link</c> are JSON objects.
/// </summary>
public record CreateNotificationResource(
    [Range(1, int.MaxValue)] int UserId,
    [Required] string Type,
    JsonElement? Params,
    JsonElement? Link);
