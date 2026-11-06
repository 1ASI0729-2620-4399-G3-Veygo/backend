using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Engagement.Domain.Model.Aggregates;

namespace Veygo.Platform.Engagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core model configuration of the Engagement bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the Engagement mappings.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    public static void ApplyEngagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Favorite>(favorite =>
        {
            favorite.HasKey(f => f.Id);
            favorite.Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
            favorite.Property(f => f.RenterId).IsRequired();
            favorite.Property(f => f.VehicleId).IsRequired();
            favorite.HasIndex(f => new { f.RenterId, f.VehicleId }).IsUnique();
        });
    }
}
