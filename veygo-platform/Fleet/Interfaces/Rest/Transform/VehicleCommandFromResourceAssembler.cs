using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Fleet.Domain.Model.ValueObjects;
using Veygo.Platform.Fleet.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;

namespace Veygo.Platform.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles vehicle commands from REST resources.
/// </summary>
/// <remarks>Invalid enum or date values throw <see cref="ArgumentException" /> (400 Bad Request).</remarks>
public static class VehicleCommandFromResourceAssembler
{
    /// <summary>Assembles a <see cref="CreateVehicleCommand" />.</summary>
    public static CreateVehicleCommand ToCreateCommandFromResource(CreateVehicleResource resource) =>
        new(resource.OwnerId,
            resource.Brand,
            resource.Model,
            resource.Year,
            EnumValueAssembler.ToEnum<EVehicleCategory>(resource.Category, "category"),
            EnumValueAssembler.ToOptionalEnum<EBodyType>(resource.BodyType, "bodyType"),
            EnumValueAssembler.ToEnum<ETransmissionType>(resource.Transmission, "transmission"),
            EnumValueAssembler.ToEnum<EFuelType>(resource.FuelType, "fuelType"),
            resource.Seats,
            resource.Doors,
            resource.PricePerDay,
            resource.PricePerWeek,
            resource.PricePerMonth,
            resource.SecurityDeposit,
            resource.Description ?? string.Empty,
            resource.Features ?? [],
            resource.Photos ?? [],
            ToLocation(resource.Location),
            resource.Published,
            resource.Plate ?? string.Empty,
            resource.Color ?? string.Empty,
            resource.Mileage,
            DateValueAssembler.ToOptionalDate(resource.LastMaintenanceAt, "lastMaintenanceAt"),
            ToInsurance(resource.Insurance),
            DateValueAssembler.ToOptionalDate(resource.TechnicalInspectionUntil, "technicalInspectionUntil"));

    /// <summary>Assembles an <see cref="UpdateVehicleCommand" /> for the vehicle with the given id.</summary>
    public static UpdateVehicleCommand ToUpdateCommandFromResource(int vehicleId, CreateVehicleResource resource)
    {
        var data = ToCreateCommandFromResource(resource);
        return new UpdateVehicleCommand(vehicleId, data.Brand, data.Model, data.Year, data.Category, data.BodyType,
            data.Transmission, data.FuelType, data.Seats, data.Doors, data.PricePerDay, data.PricePerWeek,
            data.PricePerMonth, data.SecurityDeposit, data.Description, data.Features, data.Photos, data.Location,
            data.Published, data.Plate, data.Color, data.Mileage, data.LastMaintenanceAt, data.Insurance,
            data.TechnicalInspectionUntil);
    }

    private static VehicleLocation ToLocation(VehicleLocationResource? resource) =>
        resource is null
            ? new VehicleLocation()
            : new VehicleLocation(resource.Address?.Trim() ?? string.Empty, resource.District?.Trim() ?? string.Empty,
                string.IsNullOrWhiteSpace(resource.City) ? "Lima" : resource.City.Trim(), resource.Latitude,
                resource.Longitude);

    private static VehicleInsurance? ToInsurance(VehicleInsuranceResource? resource) =>
        string.IsNullOrWhiteSpace(resource?.Provider)
            ? null
            : new VehicleInsurance(resource.Provider, DateValueAssembler.ToOptionalDate(resource.ValidUntil, "insurance.validUntil"));
}
