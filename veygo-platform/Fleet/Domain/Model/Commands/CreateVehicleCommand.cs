using Veygo.Platform.Fleet.Domain.Model.ValueObjects;

namespace Veygo.Platform.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to register a new vehicle of an owner.
/// </summary>
public record CreateVehicleCommand(
    int OwnerId,
    string Brand,
    string Model,
    int Year,
    EVehicleCategory Category,
    EBodyType? BodyType,
    ETransmissionType Transmission,
    EFuelType FuelType,
    int Seats,
    int Doors,
    decimal PricePerDay,
    decimal PricePerWeek,
    decimal PricePerMonth,
    decimal SecurityDeposit,
    string Description,
    IReadOnlyCollection<string> Features,
    IReadOnlyCollection<string> Photos,
    VehicleLocation Location,
    bool Published,
    string Plate,
    string Color,
    int Mileage,
    DateOnly? LastMaintenanceAt,
    VehicleInsurance? Insurance,
    DateOnly? TechnicalInspectionUntil);
