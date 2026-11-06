using Veygo.Platform.Notification.Application.CommandServices;
using Veygo.Platform.Notification.Application.Internal.CommandServices;
using Veygo.Platform.Notification.Application.Internal.QueryServices;
using Veygo.Platform.Notification.Application.QueryServices;
using Veygo.Platform.Notification.Domain.Repositories;
using Veygo.Platform.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Veygo.Platform.Notification.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Dependency injection configuration of the Notification bounded context.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    public static void AddNotificationContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<INotificationCommandService, NotificationCommandService>();
        builder.Services.AddScoped<INotificationQueryService, NotificationQueryService>();
    }
}
