using Veygo.Platform.Shared.Domain.Repositories;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Domain.Repositories;

/// <summary>
///     Repository of notifications.
/// </summary>
public interface INotificationRepository : IBaseRepository<NotificationAggregate>
{
    /// <summary>Lists the latest notifications of a user, most recent first (read only).</summary>
    Task<IEnumerable<NotificationAggregate>> FindLatestByUserIdAsync(int userId, int limit,
        CancellationToken cancellationToken = default);
}
