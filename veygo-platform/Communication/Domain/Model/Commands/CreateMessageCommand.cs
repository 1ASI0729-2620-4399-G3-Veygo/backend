namespace Veygo.Platform.Communication.Domain.Model.Commands;

/// <summary>
///     Command to send a message in a conversation.
/// </summary>
public record CreateMessageCommand(int ConversationId, int SenderId, string Content);
