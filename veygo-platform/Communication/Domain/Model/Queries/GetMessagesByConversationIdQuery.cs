namespace Veygo.Platform.Communication.Domain.Model.Queries;

/// <summary>
///     Query to get the messages of a conversation (oldest first).
/// </summary>
public record GetMessagesByConversationIdQuery(int ConversationId);
