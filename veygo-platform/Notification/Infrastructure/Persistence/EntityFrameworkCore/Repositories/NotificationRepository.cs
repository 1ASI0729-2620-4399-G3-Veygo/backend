using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Notification.Domain.Repositories;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="INotificationRepository" />.
/// </summary>
public class NotificationRepository(AppDbContext context)
    : BaseRepository<NotificationAggregate>(context), INotificationRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<NotificationAggregate>> FindLatestByUserIdAsync(int userId, int limit,
        CancellationToken cancellationToken = default) =>
        await Context.Set<NotificationAggregate>().AsNoTracking()
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .ThenByDescending(notification => notification.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);
}
