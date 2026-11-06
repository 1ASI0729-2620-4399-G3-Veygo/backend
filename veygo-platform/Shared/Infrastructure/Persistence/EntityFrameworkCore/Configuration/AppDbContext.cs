using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Booking.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Veygo.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Veygo.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Veygo.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Veygo.Platform.Notification.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Veygo.Platform.Reputation.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

namespace Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context for the Veygo Platform.
/// </summary>
/// <param name="options">The options for the database context.</param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Per bounded context configuration
        builder.ApplyFleetConfiguration();
        builder.ApplyBookingConfiguration();
        builder.ApplyEngagementConfiguration();
        builder.ApplyReputationConfiguration();
        builder.ApplyCommunicationConfiguration();
        builder.ApplyNotificationConfiguration();

        // General naming convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}
