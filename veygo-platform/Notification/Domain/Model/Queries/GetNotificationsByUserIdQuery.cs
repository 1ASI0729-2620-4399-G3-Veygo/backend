namespace Veygo.Platform.Notification.Domain.Model.Queries;

/// <summary>
///     Query to get the latest notifications of a user (most recent first).
/// </summary>
/// <param name="UserId">The recipient.</param>
/// <param name="Limit">Maximum number of notifications.</param>
public record GetNotificationsByUserIdQuery(int UserId, int Limit = 30);
