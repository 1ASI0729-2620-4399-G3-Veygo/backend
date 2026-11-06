using Veygo.Platform.Notification.Domain.Model.Commands;
using Veygo.Platform.Shared.Application.Model;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Application.CommandServices;

/// <summary>
///     Command service for notifications.
/// </summary>
public interface INotificationCommandService
{
    Task<Result<NotificationAggregate>> Handle(CreateNotificationCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<NotificationAggregate>> Handle(MarkNotificationAsReadCommand command,
        CancellationToken cancellationToken = default);
}
