namespace Veygo.Platform.Fleet.Domain.Model.ValueObjects;

/// <summary>
///     Insurance policy of a vehicle.
/// </summary>
/// <param name="Provider">Insurance company (e.g. Rímac, Pacífico).</param>
/// <param name="ValidUntil">Last day of coverage (optional).</param>
public record VehicleInsurance(string Provider, DateOnly? ValidUntil)
{
    /// <summary>
    ///     Gets a value indicating whether the policy covers the given date.
    /// </summary>
    public bool IsValidOn(DateOnly date) => ValidUntil.HasValue && ValidUntil.Value >= date;
}
