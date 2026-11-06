namespace Veygo.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Message of a conversation.
/// </summary>
public record MessageResource(int Id, int ConversationId, int SenderId, string Content, DateTimeOffset? SentAt, bool Read);
