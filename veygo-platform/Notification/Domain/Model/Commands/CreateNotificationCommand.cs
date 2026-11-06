using Veygo.Platform.Notification.Domain.Model.ValueObjects;

namespace Veygo.Platform.Notification.Domain.Model.Commands;

/// <summary>
///     Command to notify a user. <c>Params</c> and <c>Link</c> are JSON objects (optional).
/// </summary>
public record CreateNotificationCommand(int UserId, ENotificationType Type, string? Params, string? Link);
