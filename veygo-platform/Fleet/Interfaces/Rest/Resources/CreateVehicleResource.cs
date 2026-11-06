using System.ComponentModel.DataAnnotations;

namespace Veygo.Platform.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Data required to register (POST) or replace (PUT) a vehicle.
/// </summary>
/// <remarks>
///     Enum values use the web application format: <c>category</c> (car, suv, pickup, electric, economy, luxury),
///     <c>bodyType</c> (sedan, hatchback, suv, pickup, van or empty), <c>transmission</c> (manual, automatic),
///     <c>fuelType</c> (gasoline, diesel, hybrid, electric). Dates use <c>yyyy-MM-dd</c>.
///     <c>ownerId</c> is ignored on PUT: the owner of a vehicle cannot change.
/// </remarks>
public record CreateVehicleResource(
    [Range(1, int.MaxValue)] int OwnerId,
    [Required] string Brand,
    [Required] string Model,
    int Year,
    [Required] string Category,
    string? BodyType,
    [Required] string Transmission,
    [Required] string FuelType,
    int Seats,
    int Doors,
    decimal PricePerDay,
    decimal PricePerWeek,
    decimal PricePerMonth,
    decimal SecurityDeposit,
    string? Description,
    IReadOnlyList<string>? Features,
    IReadOnlyList<string>? Photos,
    VehicleLocationResource? Location,
    bool Published,
    string? Plate,
    string? Color,
    int Mileage,
    string? LastMaintenanceAt,
    VehicleInsuranceResource? Insurance,
    string? TechnicalInspectionUntil);
