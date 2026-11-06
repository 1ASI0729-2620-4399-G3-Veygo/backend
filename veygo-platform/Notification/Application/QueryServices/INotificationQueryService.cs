using Veygo.Platform.Notification.Domain.Model.Queries;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Application.QueryServices;

/// <summary>
///     Query service for notifications.
/// </summary>
public interface INotificationQueryService
{
    Task<IEnumerable<NotificationAggregate>> Handle(GetNotificationsByUserIdQuery query,
        CancellationToken cancellationToken = default);

    Task<NotificationAggregate?> Handle(GetNotificationByIdQuery query, CancellationToken cancellationToken = default);
}
