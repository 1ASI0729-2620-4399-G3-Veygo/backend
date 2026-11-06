namespace Veygo.Platform.Engagement.Domain.Model.Queries;

/// <summary>
///     Query to get the favorites of a renter (most recent first).
/// </summary>
public record GetFavoritesByRenterIdQuery(int RenterId);
