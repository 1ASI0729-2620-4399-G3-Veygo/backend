namespace Veygo.Platform.Notification.Domain.Model.ValueObjects;

/// <summary>
///     Kind of notification. The web application translates the message from the type and its params.
/// </summary>
public enum ENotificationType
{
    BookingRequested,
    BookingConfirmed,
    BookingRejected,
    BookingCancelled,
    MessageReceived,
    ReviewReceived
}
