namespace Veygo.Platform.Notification.Interfaces.Rest.Resources;

/// <summary>
///     Partial update of a notification. Only <c>read: true</c> is supported.
/// </summary>
public record UpdateNotificationResource(bool? Read);
