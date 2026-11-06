using Veygo.Platform.Notification.Application.QueryServices;
using Veygo.Platform.Notification.Domain.Model.Queries;
using Veygo.Platform.Notification.Domain.Repositories;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Application.Internal.QueryServices;

/// <summary>
///     Query service implementation for notifications.
/// </summary>
public class NotificationQueryService(INotificationRepository notificationRepository) : INotificationQueryService
{
    public async Task<IEnumerable<NotificationAggregate>> Handle(GetNotificationsByUserIdQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationRepository.FindLatestByUserIdAsync(query.UserId, query.Limit, cancellationToken);

    public async Task<NotificationAggregate?> Handle(GetNotificationByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationRepository.FindByIdAsNoTrackingAsync(query.NotificationId, cancellationToken);
}
