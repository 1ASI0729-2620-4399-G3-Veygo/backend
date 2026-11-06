using System.Text.Json;

namespace Veygo.Platform.Notification.Interfaces.Rest.Resources;

/// <summary>
///     Notification of a user. <c>type</c> is booking-requested, booking-confirmed, booking-rejected,
///     booking-cancelled, message-received or review-received.
/// </summary>
public record NotificationResource(
    int Id,
    int UserId,
    string Type,
    JsonElement Params,
    JsonElement? Link,
    bool Read,
    DateTimeOffset? CreatedAt);
