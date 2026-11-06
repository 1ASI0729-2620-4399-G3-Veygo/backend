using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Fleet.Domain.Model.ValueObjects;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Fleet.Domain.Model.Aggregates;

/// <summary>
///     Vehicle aggregate root. A vehicle belongs to an owner and can be published in the catalog
///     to be rented by renters.
/// </summary>
public class Vehicle : AggregateRoot, IAuditableEntity
{
    /// <summary>Oldest model year accepted by the platform.</summary>
    public const int MinimumYear = 1990;

    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected Vehicle()
    {
        Brand = string.Empty;
        Model = string.Empty;
        Description = string.Empty;
        Plate = string.Empty;
        Color = string.Empty;
        Features = [];
        Photos = [];
        Location = new VehicleLocation();
    }

    /// <summary>
    ///     Creates a vehicle from the <see cref="CreateVehicleCommand" />.
    /// </summary>
    /// <param name="command">The command with the vehicle data.</param>
    /// <exception cref="ArgumentException">When a domain invariant is violated.</exception>
    public Vehicle(CreateVehicleCommand command) : this()
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.OwnerId <= 0) throw new ArgumentException("A vehicle must belong to an owner.", nameof(command));

        OwnerId = command.OwnerId;
        SetDetails(command.Brand, command.Model, command.Year, command.Category, command.BodyType, command.Transmission,
            command.FuelType, command.Seats, command.Doors, command.PricePerDay, command.PricePerWeek,
            command.PricePerMonth, command.SecurityDeposit, command.Description, command.Features, command.Photos,
            command.Location, command.Plate, command.Color, command.Mileage, command.LastMaintenanceAt,
            command.Insurance, command.TechnicalInspectionUntil);
        Published = command.Published;
    }

    public int Id { get; private set; }
    public int OwnerId { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public EVehicleCategory Category { get; private set; }
    public EBodyType? BodyType { get; private set; }
    public ETransmissionType Transmission { get; private set; }
    public EFuelType FuelType { get; private set; }
    public int Seats { get; private set; }
    public int Doors { get; private set; }
    public decimal PricePerDay { get; private set; }
    public decimal PricePerWeek { get; private set; }
    public decimal PricePerMonth { get; private set; }
    public decimal SecurityDeposit { get; private set; }
    public string Description { get; private set; }
    public List<string> Features { get; private set; }
    public List<string> Photos { get; private set; }
    public VehicleLocation Location { get; private set; }
    public bool Published { get; private set; }
    public double Rating { get; private set; }
    public int ReviewsCount { get; private set; }
    public string Plate { get; private set; }
    public string Color { get; private set; }
    public int Mileage { get; private set; }
    public DateOnly? LastMaintenanceAt { get; private set; }
    public VehicleInsurance? Insurance { get; private set; }
    public DateOnly? TechnicalInspectionUntil { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Gets the name shown to users (e.g. "Toyota RAV4 Hybrid 2023").</summary>
    public string DisplayName => $"{Brand} {Model} {Year}";

    /// <summary>
    ///     Replaces the editable data of the vehicle.
    /// </summary>
    /// <param name="command">The command with the new data.</param>
    /// <exception cref="ArgumentException">When a domain invariant is violated.</exception>
    public void Update(UpdateVehicleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        SetDetails(command.Brand, command.Model, command.Year, command.Category, command.BodyType, command.Transmission,
            command.FuelType, command.Seats, command.Doors, command.PricePerDay, command.PricePerWeek,
            command.PricePerMonth, command.SecurityDeposit, command.Description, command.Features, command.Photos,
            command.Location, command.Plate, command.Color, command.Mileage, command.LastMaintenanceAt,
            command.Insurance, command.TechnicalInspectionUntil);
        Published = command.Published;
    }

    /// <summary>Publishes the vehicle in the catalog.</summary>
    public void Publish() => Published = true;

    /// <summary>Removes the vehicle from the catalog.</summary>
    public void Unpublish() => Published = false;

    /// <summary>
    ///     Updates the rating summary of the vehicle.
    /// </summary>
    /// <param name="rating">Average rating from 0 to 5.</param>
    /// <param name="reviewsCount">Number of reviews.</param>
    /// <exception cref="ArgumentException">When the values are out of range.</exception>
    public void UpdateReputation(double rating, int reviewsCount)
    {
        if (rating is < 0 or > 5) throw new ArgumentException("The rating must be between 0 and 5.", nameof(rating));
        if (reviewsCount < 0)
            throw new ArgumentException("The reviews count cannot be negative.", nameof(reviewsCount));
        Rating = Math.Round(rating, 1);
        ReviewsCount = reviewsCount;
    }

    /// <summary>Gets a value indicating whether the vehicle belongs to the given owner.</summary>
    public bool IsOwnedBy(int ownerId) => OwnerId == ownerId;

    private void SetDetails(string brand, string model, int year, EVehicleCategory category, EBodyType? bodyType,
        ETransmissionType transmission, EFuelType fuelType, int seats, int doors, decimal pricePerDay,
        decimal pricePerWeek, decimal pricePerMonth, decimal securityDeposit, string? description,
        IEnumerable<string>? features, IEnumerable<string>? photos, VehicleLocation? location, string? plate,
        string? color, int mileage, DateOnly? lastMaintenanceAt, VehicleInsurance? insurance,
        DateOnly? technicalInspectionUntil)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(brand);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        var maximumYear = DateTime.UtcNow.Year + 1;
        if (year < MinimumYear || year > maximumYear)
            throw new ArgumentException($"The vehicle year must be between {MinimumYear} and {maximumYear}.",
                nameof(year));
        if (seats < 1) throw new ArgumentException("A vehicle must have at least one seat.", nameof(seats));
        if (doors < 0) throw new ArgumentException("The number of doors cannot be negative.", nameof(doors));
        if (pricePerDay <= 0)
            throw new ArgumentException("The price per day must be greater than zero.", nameof(pricePerDay));
        if (pricePerWeek < 0 || pricePerMonth < 0 || securityDeposit < 0)
            throw new ArgumentException("Prices and security deposit cannot be negative.", nameof(pricePerWeek));
        if (mileage < 0) throw new ArgumentException("The mileage cannot be negative.", nameof(mileage));

        Brand = brand.Trim();
        Model = model.Trim();
        Year = year;
        Category = category;
        BodyType = bodyType;
        Transmission = transmission;
        FuelType = fuelType;
        Seats = seats;
        Doors = doors;
        PricePerDay = pricePerDay;
        PricePerWeek = pricePerWeek;
        PricePerMonth = pricePerMonth;
        SecurityDeposit = securityDeposit;
        Description = description?.Trim() ?? string.Empty;
        Features = VehicleFeatures.Normalize(features);
        Photos = (photos ?? []).Select(photo => photo.Trim()).Where(photo => photo.Length > 0).Distinct().ToList();
        Location = location ?? new VehicleLocation();
        Plate = (plate ?? string.Empty).Trim().ToUpperInvariant();
        Color = color?.Trim() ?? string.Empty;
        Mileage = mileage;
        LastMaintenanceAt = lastMaintenanceAt;
        Insurance = string.IsNullOrWhiteSpace(insurance?.Provider) ? null : insurance with { Provider = insurance.Provider.Trim() };
        TechnicalInspectionUntil = technicalInspectionUntil;
    }
}
