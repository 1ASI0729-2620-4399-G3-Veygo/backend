namespace Veygo.Platform.Reputation.Domain.Model.Queries;

/// <summary>
///     Query to get the reviews written by a renter.
/// </summary>
public record GetReviewsByRenterIdQuery(int RenterId);
