namespace Veygo.Platform.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Insurance policy of a vehicle. <c>validUntil</c> uses the <c>yyyy-MM-dd</c> format.
/// </summary>
public record VehicleInsuranceResource(string? Provider, string? ValidUntil);
