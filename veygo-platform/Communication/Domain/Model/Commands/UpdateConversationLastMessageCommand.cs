namespace Veygo.Platform.Communication.Domain.Model.Commands;

/// <summary>
///     Command to update the preview of the last message of a conversation.
/// </summary>
/// <remarks>
///     Sending a message already updates the conversation; this command keeps compatibility with clients
///     that update it explicitly.
/// </remarks>
public record UpdateConversationLastMessageCommand(int ConversationId, string Preview, DateTimeOffset LastMessageAt);
