using Microsoft.EntityFrameworkCore;
using NotificationAggregate = Veygo.Platform.Notification.Domain.Model.Aggregates.Notification;

namespace Veygo.Platform.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core model configuration of the Notification bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the Notification mappings.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    public static void ApplyNotificationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<NotificationAggregate>(notification =>
        {
            notification.HasKey(n => n.Id);
            notification.Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
            notification.Property(n => n.UserId).IsRequired();
            notification.Property(n => n.Type).IsRequired().HasConversion<string>().HasMaxLength(30);
            notification.Property(n => n.Params).IsRequired().HasColumnType("text");
            notification.Property(n => n.Link).HasColumnType("text");
            notification.Property(n => n.Read).IsRequired();
            notification.HasIndex(n => n.UserId);
        });
    }
}
