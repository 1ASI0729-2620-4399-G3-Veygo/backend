namespace Veygo.Platform.Booking.Domain.Model.ValueObjects;

/// <summary>
///     Rental terms of a vehicle as seen by the Booking context (obtained from Fleet through its ACL).
/// </summary>
/// <param name="VehicleId">The vehicle id.</param>
/// <param name="OwnerId">The owner of the vehicle.</param>
/// <param name="PricePerDay">The current price per day.</param>
/// <param name="Published">Whether the vehicle is published in the catalog.</param>
public record VehicleRentalTerms(int VehicleId, int OwnerId, decimal PricePerDay, bool Published);