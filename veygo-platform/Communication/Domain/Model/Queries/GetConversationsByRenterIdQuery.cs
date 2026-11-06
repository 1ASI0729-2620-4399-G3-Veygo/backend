namespace Veygo.Platform.Communication.Domain.Model.Queries;

/// <summary>
///     Query to get the conversations of a renter (most recent activity first).
/// </summary>
public record GetConversationsByRenterIdQuery(int RenterId);
