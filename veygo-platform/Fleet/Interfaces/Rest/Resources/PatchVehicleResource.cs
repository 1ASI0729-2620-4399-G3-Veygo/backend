namespace Veygo.Platform.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Partial update of a vehicle: publication state and/or rating summary. Omitted fields are not changed.
/// </summary>
public record PatchVehicleResource(bool? Published, double? Rating, int? ReviewsCount);
