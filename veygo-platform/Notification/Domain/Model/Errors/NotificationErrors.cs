using Veygo.Platform.Shared.Domain.Model.Errors;

namespace Veygo.Platform.Notification.Domain.Model.Errors;

/// <summary>
///     Error catalog of the Notification bounded context.
/// </summary>
public static class NotificationErrors
{
    public static readonly Error NotificationNotFound =
        Error.NotFound("Notification.NotificationNotFound", "The notification was not found.");
}
