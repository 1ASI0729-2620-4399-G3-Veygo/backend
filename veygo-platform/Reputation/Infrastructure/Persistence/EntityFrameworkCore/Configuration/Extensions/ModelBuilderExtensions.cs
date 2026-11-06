using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Reputation.Domain.Model.Aggregates;

namespace Veygo.Platform.Reputation.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core model configuration of the Reputation bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the Reputation mappings.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    public static void ApplyReputationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Review>(review =>
        {
            review.HasKey(r => r.Id);
            review.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
            review.Property(r => r.BookingId).IsRequired();
            review.Property(r => r.VehicleId).IsRequired();
            review.Property(r => r.OwnerId).IsRequired();
            review.Property(r => r.RenterId).IsRequired();
            review.Property(r => r.Rating).IsRequired();
            review.Property(r => r.Comment).HasMaxLength(Review.MaxCommentLength);
            review.Ignore(r => r.IsPositive);

            review.HasIndex(r => r.BookingId).IsUnique();
            review.HasIndex(r => r.OwnerId);
            review.HasIndex(r => r.VehicleId);
        });
    }
}
