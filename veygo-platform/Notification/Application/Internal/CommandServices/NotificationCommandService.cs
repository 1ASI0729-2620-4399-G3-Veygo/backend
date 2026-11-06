using Veygo.Platform.Notification.Application.CommandServices;
using Veygo.Platform.Notification.Domain.Model.Commands;
using Veygo.Platform.Notification.Domain.Model.Errors;
using Veygo.Platform.Notification.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Repositories;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for notifications.
/// </summary>
public class NotificationCommandService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    : INotificationCommandService
{
    /// <inheritdoc />
    public async Task<Result<NotificationAggregate>> Handle(CreateNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        var notification = new NotificationAggregate(command);
        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<NotificationAggregate>.Success(notification);
    }

    /// <inheritdoc />
    public async Task<Result<NotificationAggregate>> Handle(MarkNotificationAsReadCommand command,
        CancellationToken cancellationToken = default)
    {
        var notification = await notificationRepository.FindByIdAsync(command.NotificationId, cancellationToken);
        if (notification is null)
            return Result<NotificationAggregate>.Failure(NotificationErrors.NotificationNotFound);

        notification.MarkAsRead();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<NotificationAggregate>.Success(notification);
    }
}
