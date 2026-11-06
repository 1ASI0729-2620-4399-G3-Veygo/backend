namespace Veygo.Platform.Reputation.Domain.Model.Queries;

/// <summary>
///     Query to get the reviews received by an owner.
/// </summary>
public record GetReviewsByOwnerIdQuery(int OwnerId);
