using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core model configuration of the Booking bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the Booking mappings.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    public static void ApplyBookingConfiguration(this ModelBuilder builder)
    {
        builder.Entity<BookingAggregate>(booking =>
        {
            booking.HasKey(b => b.Id);
            booking.Property(b => b.Id).IsRequired().ValueGeneratedOnAdd();
            booking.Property(b => b.VehicleId).IsRequired();
            booking.Property(b => b.RenterId).IsRequired();
            booking.Property(b => b.OwnerId).IsRequired();
            booking.Property(b => b.StartDate).IsRequired().HasDateConversion();
            booking.Property(b => b.EndDate).IsRequired().HasDateConversion();
            booking.Property(b => b.PricePerDay).IsRequired().HasPrecision(10, 2);
            booking.Property(b => b.TotalPrice).IsRequired().HasPrecision(10, 2);
            booking.Property(b => b.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
            booking.Property(b => b.PaymentMethod).IsRequired().HasConversion<string>().HasMaxLength(20);
            booking.Ignore(b => b.Days);
            booking.Ignore(b => b.LastNight);
            booking.Ignore(b => b.IsActive);

            booking.HasIndex(b => b.VehicleId);
            booking.HasIndex(b => b.RenterId);
            booking.HasIndex(b => b.OwnerId);
        });
    }
}