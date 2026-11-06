namespace Veygo.Platform.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Vehicle returned by the API. Dates use the <c>yyyy-MM-dd</c> format (empty string when not set).
/// </summary>
public record VehicleResource(
    int Id,
    int OwnerId,
    string Brand,
    string Model,
    int Year,
    string Category,
    string BodyType,
    string Transmission,
    string FuelType,
    int Seats,
    int Doors,
    decimal PricePerDay,
    decimal PricePerWeek,
    decimal PricePerMonth,
    decimal SecurityDeposit,
    string Description,
    IReadOnlyList<string> Features,
    IReadOnlyList<string> Photos,
    VehicleLocationResource Location,
    bool Published,
    double Rating,
    int ReviewsCount,
    string Plate,
    string Color,
    int Mileage,
    string LastMaintenanceAt,
    VehicleInsuranceResource? Insurance,
    string TechnicalInspectionUntil);
