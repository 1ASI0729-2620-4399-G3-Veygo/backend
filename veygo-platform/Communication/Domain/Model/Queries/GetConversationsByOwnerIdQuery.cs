namespace Veygo.Platform.Communication.Domain.Model.Queries;

/// <summary>
///     Query to get the conversations of an owner (most recent activity first).
/// </summary>
public record GetConversationsByOwnerIdQuery(int OwnerId);
