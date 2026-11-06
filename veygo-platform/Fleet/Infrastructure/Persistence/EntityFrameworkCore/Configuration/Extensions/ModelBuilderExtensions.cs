using Microsoft.EntityFrameworkCore;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace Veygo.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core model configuration of the Fleet bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the Fleet mappings (vehicles and blocked dates).
    /// </summary>
    /// <param name="builder">The model builder.</param>
    public static void ApplyFleetConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Vehicle>(vehicle =>
        {
            vehicle.HasKey(v => v.Id);
            vehicle.Property(v => v.Id).IsRequired().ValueGeneratedOnAdd();
            vehicle.Property(v => v.OwnerId).IsRequired();
            vehicle.Property(v => v.Brand).IsRequired().HasMaxLength(60);
            vehicle.Property(v => v.Model).IsRequired().HasMaxLength(80);
            vehicle.Property(v => v.Year).IsRequired();
            vehicle.Property(v => v.Category).IsRequired().HasConversion<string>().HasMaxLength(20);
            vehicle.Property(v => v.BodyType).HasConversion<string>().HasMaxLength(20);
            vehicle.Property(v => v.Transmission).IsRequired().HasConversion<string>().HasMaxLength(20);
            vehicle.Property(v => v.FuelType).IsRequired().HasConversion<string>().HasMaxLength(20);
            vehicle.Property(v => v.PricePerDay).IsRequired().HasPrecision(10, 2);
            vehicle.Property(v => v.PricePerWeek).IsRequired().HasPrecision(10, 2);
            vehicle.Property(v => v.PricePerMonth).IsRequired().HasPrecision(10, 2);
            vehicle.Property(v => v.SecurityDeposit).IsRequired().HasPrecision(10, 2);
            vehicle.Property(v => v.Description).HasMaxLength(2000);
            vehicle.Property(v => v.Features).HasJsonConversion().HasColumnType("longtext");
            vehicle.Property(v => v.Photos).HasJsonConversion().HasColumnType("longtext");
            vehicle.Property(v => v.Plate).HasMaxLength(10);
            vehicle.Property(v => v.Color).HasMaxLength(30);
            vehicle.Property(v => v.LastMaintenanceAt).HasDateConversion();
            vehicle.Property(v => v.TechnicalInspectionUntil).HasDateConversion();
            vehicle.Ignore(v => v.DisplayName);

            vehicle.OwnsOne(v => v.Location, location =>
            {
                location.Property(l => l.Address).HasColumnName("location_address").HasMaxLength(200);
                location.Property(l => l.District).HasColumnName("location_district").HasMaxLength(60);
                location.Property(l => l.City).HasColumnName("location_city").HasMaxLength(60);
                location.Property(l => l.Latitude).HasColumnName("location_latitude");
                location.Property(l => l.Longitude).HasColumnName("location_longitude");
                location.Ignore(l => l.HasCoordinates);
            });
            vehicle.Navigation(v => v.Location).IsRequired();

            vehicle.OwnsOne(v => v.Insurance, insurance =>
            {
                insurance.Property(i => i.Provider).HasColumnName("insurance_provider").HasMaxLength(60);
                insurance.Property(i => i.ValidUntil).HasColumnName("insurance_valid_until").HasDateConversion();
            });

            vehicle.HasIndex(v => v.OwnerId);
        });

        builder.Entity<BlockedDate>(blockedDate =>
        {
            blockedDate.HasKey(b => b.Id);
            blockedDate.Property(b => b.Id).IsRequired().ValueGeneratedOnAdd();
            blockedDate.Property(b => b.VehicleId).IsRequired();
            blockedDate.Property(b => b.Date).IsRequired().HasDateConversion();
            blockedDate.HasIndex(b => new { b.VehicleId, b.Date }).IsUnique();
        });
    }
}
