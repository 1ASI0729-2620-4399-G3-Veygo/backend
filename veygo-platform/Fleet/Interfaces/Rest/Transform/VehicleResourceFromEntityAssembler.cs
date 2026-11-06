using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;

namespace Veygo.Platform.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="VehicleResource" /> from a <see cref="Vehicle" />.
/// </summary>
public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle entity) =>
        new(entity.Id,
            entity.OwnerId,
            entity.Brand,
            entity.Model,
            entity.Year,
            EnumValueAssembler.ToValue(entity.Category),
            EnumValueAssembler.ToValue(entity.BodyType),
            EnumValueAssembler.ToValue(entity.Transmission),
            EnumValueAssembler.ToValue(entity.FuelType),
            entity.Seats,
            entity.Doors,
            entity.PricePerDay,
            entity.PricePerWeek,
            entity.PricePerMonth,
            entity.SecurityDeposit,
            entity.Description,
            entity.Features.ToList(),
            entity.Photos.ToList(),
            new VehicleLocationResource(entity.Location.Address, entity.Location.District, entity.Location.City,
                entity.Location.Latitude, entity.Location.Longitude),
            entity.Published,
            entity.Rating,
            entity.ReviewsCount,
            entity.Plate,
            entity.Color,
            entity.Mileage,
            DateValueAssembler.ToValue(entity.LastMaintenanceAt),
            entity.Insurance is null
                ? null
                : new VehicleInsuranceResource(entity.Insurance.Provider,
                    DateValueAssembler.ToValue(entity.Insurance.ValidUntil)),
            DateValueAssembler.ToValue(entity.TechnicalInspectionUntil));
}
